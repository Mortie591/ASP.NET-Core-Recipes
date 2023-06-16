using HtmlAgilityPack;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Services;
using OurRecipes.Services.Models.ScraperDtos;
using System.Text;

namespace DataSeeder
{
    public class ScraperService:InitialDataService, IScraperService
    {
        
        private readonly List<string> urls = new List<String>();
        private readonly List<string> collections = new List<String>() 
        { "easy-recipes", "easy-vegetarian-recipes","chocolate-dessert-recipes"};

        public ScraperService(ApplicationDbContext db) 
            : base(db)
        {
        }

        public async Task PopulateData()
        {
           var recipeDtos = await InitializeScraping();

            foreach(var recipeDto in recipeDtos)
            {
                if (recipeDto != null)
                {
                    Recipe recipe = new Recipe
                    {
                        Title = recipeDto.Title,
                        Description = recipeDto.Description,
                        Servings = recipeDto.Servings==null?"": recipeDto.Servings,
                        PrepTime = recipeDto.PrepTime,
                        CookTime = recipeDto.CookTime,
                        ImageUrl = recipeDto?.ImageUrl,
                        OriginalUrl = recipeDto.OriginalUrl,
                        CreatedOnDate = DateTime.Now,
                        Instructions = recipeDto.Instructions,
                        Categories = recipeDto.Categories.Select(GetOrCreateCategory).ToList(),
                        Tags = recipeDto.Tags.Select(GetOrCreateTag).ToList(),
                        Components = GetOrCreateComponents(recipeDto.Components),
                        Nutrients = recipeDto.Nutrients.Select(x => GetOrCreateNutrient(x.Name, x.Quantity, x.Unit)).ToList()
                    };

                    if (recipe.Components.Any(c => c.Quantity == null))
                    {
                        Console.WriteLine($"{recipe.Title} is invalid");
                        continue;
                    }
                    else
                    {
                        if (!this.context.Recipes.Select(x => x.Title).Contains(recipe.Title))
                        {
                            if (!this.recipes.Select(x => x.Title).Contains(recipe.Title))
                            {
                                this.context.Recipes.Add(recipe);

                                this.context.SaveChanges();

                            }
                        }
                    }
                }
            }
            
        }
        
        public async Task<ICollection<RecipeDto>> InitializeScraping()
        {
            var recipes = new List<RecipeDto>();
            var collectionUrls = new List<string>();

            foreach (var collectionName in collections)
            {
                var retrievedUrls = GetRecipeUrlsFromCollection(collectionName, collectionUrls);
                this.urls.AddRange(retrievedUrls);
            }
            Console.WriteLine(this.urls.Count);

            foreach (var a in urls)
            {
                var url = $"https://www.bbcgoodfood.com{a}";
                try
                {
                    var recipeDto = await GatherRecipe(url);
                    recipes.Add(recipeDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{url} - {ex.Message}");
                }
            }

            Console.WriteLine(recipes.Count);
            return recipes;
        }

        //TODO: ImagesService to populate thumbnails where missing
        //private string GetImageUrl (string title)
        //{
        //    //https://pixabay.com/api/docs/#api_search_images
        //    return null;
        //}
        private static async Task<RecipeDto> GatherRecipe(string url)
        {
            var recipeDto = new RecipeDto();

            var doc = GetDocument(url);
            var topSection = doc.DocumentNode.SelectSingleNode("//div[@class='container post-header__container post-header__container--masthead-layout']");
            
            recipeDto.Title = topSection.SelectSingleNode("//h1").InnerHtml;
            recipeDto.OriginalUrl = url;
            
            var planList = topSection.SelectSingleNode("//ul[@class='post-header__row post-header__planning list list--horizontal']");

            var planInfo = planList.SelectNodes("//div[@class='icon-with-text__children']");
            foreach (var item in planInfo)
            {
                string itemText = item.InnerText;

                if (itemText.Contains("Prep") || itemText.Contains("Cook"))
                {
                    var timeList = itemText.Split(new string[] { "Prep", "Cook" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var time in timeList)
                    {
                        string[] timeAsStringArr = time.Split(":");
                        if (timeAsStringArr[0].Contains("Prep"))
                        {
                            recipeDto.PrepTime = timeAsStringArr[1];
                        }
                        else if (timeAsStringArr[0].Contains("Cook"))
                        {
                            recipeDto.CookTime = timeAsStringArr[1];
                        }
                    }
                    continue;
                }
                if (itemText.Contains("Serves"))
                {
                    var itemTextAsString = itemText.Split();
                    recipeDto.Servings = itemTextAsString[1];
                }
                else
                {
                    recipeDto.Categories.Add(itemText);
                }

            }
            recipeDto.Description = topSection.SelectSingleNode("//div[@class='editor-content mt-sm pr-xxs hidden-print']/p").InnerText;

            var tagElements = topSection.SelectNodes("//ul[@class='terms-icons-list d-flex post-header__term-icons-list mt-sm hidden-print list list--horizontal']");
            if (tagElements != null && tagElements.Count > 0)
            {
                foreach (var item in tagElements)
                {
                    var spanText = item.SelectSingleNode("//span[@class='terms-icons-list__text d-flex align-items-center']").InnerText;
                    recipeDto.Tags.Add(spanText);
                }
            }

            var nutrientsRows = topSection.SelectNodes("//table/tbody/tr");
            if (nutrientsRows != null)
            {
                foreach (var row in nutrientsRows)
                {
                    var key = row.ChildNodes.Where(x => x.HasClass("key-value-blocks__key")).FirstOrDefault().InnerHtml;
                    var values = row.ChildNodes.Where(x => x.HasClass("key-value-blocks__value")).FirstOrDefault().InnerHtml;
                    var valuesFormatted = values.Split("<!-- -->", StringSplitOptions.RemoveEmptyEntries);
                    recipeDto.Nutrients.Add(new NutrientDto
                    {
                        Name = key,
                        Quantity = valuesFormatted[0],
                        Unit = valuesFormatted.Length > 1 ? valuesFormatted[1] : null
                    });
                }
            }

            var rowRecipeInstructions = doc.DocumentNode.SelectSingleNode("//div[@class='row recipe__instructions']").ChildNodes;
            var ingredientsList = rowRecipeInstructions[0].SelectSingleNode("//section/ul").ChildNodes;
            var instructionsList = rowRecipeInstructions[1].SelectSingleNode("//section/div/ul").ChildNodes;

            foreach (var el in ingredientsList)
            {
                var ingredientElements = el.ChildNodes;

                string[] units = new string[] { "g", "tbsp", "tsp", "oz", "lb", "ml" };
                string quantity = "";
                string unit = "";
                string ingredientName = "";

                string firstElement = ingredientElements[0].InnerText;
                string secondElement = "";

                if (ingredientElements.Count > 1)
                {
                    secondElement = ingredientElements[1].InnerText;
                }

                bool startsWithNumber = !Char.IsLetter(firstElement[0]); 
                bool elementContainsUnit = units.Any(x => firstElement.Contains(x));

                if (startsWithNumber && elementContainsUnit)
                {
                    char[] quantityArr = firstElement.ToCharArray();
                    StringBuilder qtyBuilder = new StringBuilder();
                    StringBuilder unitBuilder = new StringBuilder();

                    for (int i = 0; i < quantityArr.Length; i++)
                    {
                        if (Char.IsNumber(quantityArr[i]))
                        {
                            qtyBuilder.Append(quantityArr[i]);
                        }
                        else if (Char.IsLetter(quantityArr[i]))
                        {
                            unitBuilder.Append(quantityArr[i]);
                        }
                    }
                    quantity = qtyBuilder.ToString();
                    unit = unitBuilder.ToString();
                }
                else if (startsWithNumber && !elementContainsUnit)
                {
                    var firstElementArr = firstElement.Split(" ").ToArray();
                    quantity = firstElementArr[0];
                }
                else
                {
                    ingredientName = firstElement;
                    if (ingredientName.Contains(","))
                    {
                        ingredientName.Replace(",", "");
                    }
                }

                if (secondElement.Length > 0)
                {
                    bool secondElementIsUnit = secondElement.Equals(unit, StringComparison.OrdinalIgnoreCase);
                    if (!String.IsNullOrEmpty(quantity) && String.IsNullOrEmpty(unit) && secondElementIsUnit)
                    {
                        if(secondElement.Contains("g") && secondElement.Length == 1 || secondElement.Contains("gram"))
                        {
                            unit = secondElement;
                            ingredientName = ingredientElements[2]?.InnerText;
                        }else if (!secondElement.Contains("g"))
                        {
                            unit = secondElement;
                            ingredientName = ingredientElements[2]?.InnerText;
                        }
                        else
                        {
                            ingredientName = secondElement;
                        }
                        
                        if (ingredientName.Contains(","))
                        {
                            ingredientName.Replace(",", "");
                        }
                    }
                    else
                    {
                        ingredientName = secondElement;
                        if (ingredientName.Contains(","))
                        {
                            ingredientName.Replace(",", "");
                        }
                    }
                }

                recipeDto.Components.Add(new ComponentDto
                {
                    Text = el.InnerText,
                    Quantity = quantity.TrimEnd(),
                    Unit = unit.TrimEnd(),
                    IngredientName = ingredientName.TrimEnd()
                });
            }

            StringBuilder instructions = new StringBuilder();

            for (int i = 0; i < instructionsList.Count; i++)
            {
                var paragraph = instructionsList[i].LastChild.InnerText;
                instructions.AppendLine($"{paragraph}");
            }
            recipeDto.Instructions = instructions.ToString();

            return recipeDto;
        }

        private static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        private static ICollection<string> GetRecipeUrlsFromCollection(string collectionName, List<string> urls)
        {
            int count = 0;
            while (count < 5)
            {
                string collectionUrl = $"https://www.bbcgoodfood.com/recipes/collection/{collectionName}?page={++count}";
                var doc = GetDocument(collectionUrl);
                var collectionRecipeList = doc.DocumentNode.SelectNodes("//div[@class='dynamic-list dynamic-list--separated']/ul/li/div/article/div/a");
                
                if (collectionRecipeList == null) break;
                
                for (int i = 0; i < collectionRecipeList.Count; i += 2)
                {
                    var a = collectionRecipeList[i];
                    var href = a.GetAttributeValue("href", "n/a");
                    if (href.Contains("/recipes/"))
                    {
                        urls.Add(href);
                    }
                }
            }
            return urls;
        }

    }
}
