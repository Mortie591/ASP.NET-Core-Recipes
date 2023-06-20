/**
* Template Name: Yummy
* Updated: Mar 10 2023 with Bootstrap v5.2.3
* Template URL: https://bootstrapmade.com/yummy-bootstrap-restaurant-website-template/
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/


function AddNutrient() {
    let parent = event.target.parentNode;
    let liElements = parent.querySelectorAll('li');
    let nutrientsIndex = liElements.length;

    let nutrientRow = document.createElement('li');
    nutrientRow.setAttribute('name', 'nutrients-row');
    nutrientRow.setAttribute('class', 'row mt-3 mb-3');
    nutrientRow.setAttribute('id', 'nutrients-row');
    nutrientRow.innerHTML = `<div class="col-lg-4 col-md-6">
                                      <label><strong>Nutrient Quantity</strong></label>
                                      <input name="Nutrients[${nutrientsIndex}].Quantity" class="form-control" placeholder="25g">
                                      <span asp-validation-for="Nutrients[${nutrientsIndex}].Quantity" class="text-danger"></span>
                                  </div>
                                  <div class="col-lg-4 col-md-6">
                                            <label><strong>Units</strong></label>
                                            <select asp-for="Nutrients[[${nutrientsIndex}].UnitName" class="form-control">
                                                <option></option>
                                                <option>g</option>
                                            </select>
                                        </div>
                                  <div class="col-lg-4 col-md-6">
                                      <label><strong>Nutrient Name</strong></label>
                                  <input name="Nutrients[${nutrientsIndex}].Name" class="form-control" placeholder="Carbs">
                                  <span asp-validation-for="Nutrients[${nutrientsIndex}].Name" class="text-danger"></span>
                                  </div>
                                  `;
    
    const AddNutrientRow = function (parentNode) {
        parentNode.insertBefore(nutrientRow, event.target);
        console.log("Added new nutrient row");
        document.querySelector('#remove-nutrient').classList.remove('disabled');
    };
    AddNutrientRow(parent);
}


//Add ingredients
function AddIngredient() {
    let parent = event.target.parentNode;
    let liElements = parent.querySelectorAll('li');
    let ingredientsIndex = liElements.length;

    let ingreedientRow = document.createElement('li');
    ingreedientRow.setAttribute('name', 'ingredients-row');
    ingreedientRow.setAttribute('class', 'row mt-3 mb-3');
    ingreedientRow.setAttribute('id', 'ingredients-row');
    ingreedientRow.innerHTML = `<div class="col-lg-4 col-md-6">
                                        <label><strong>Ingredient Name</strong></label>
                                        <input name="Components[${ingredientsIndex}].IngredientName" class="form-control" placeholder="Pork chops">
                                        <span asp-validation-for="Components[${ingredientsIndex}].IngredientName" class="text-danger"></span>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <label><strong>Ingredient Quantity</strong></label>
                                        <input name="Components[${ingredientsIndex}].Quantity" class="form-control" placeholder="200">
                                        <span asp-validation-for="Components[${ingredientsIndex}].Quantity" class="text-danger"></span>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <label><strong>Units</strong></label>
                                        <select name="Components[${ingredientsIndex}].Unit" class="form-control">
                                            <option>---</option>
                                            <option>gr</option>
                                            <option>kg</option>
                                            <option>ml</option>
                                            <option>l</option>
                                            <option>tbsp</option>
                                            <option>tsp</option>
                                            <option>cup</option>
                                            <option>oz</option>
                                        </select>
                                    </div>`;

    const AddIngredientRow = function (parentNode) {
        parentNode.insertBefore(ingreedientRow, event.target);
        console.log("Added new ingredient row");
        document.querySelector('#remove-ingredient').classList.remove('disabled');
    };
    AddIngredientRow(parent);
}

function AddSection() {
    let parent = event.target.parentNode.parentNode;
    let ulElements = parent.querySelectorAll("ul[name='sections']");
    
    let sectionsIndex = ulElements.length;
    let sectionElement = document.createElement('ul');
    sectionElement.setAttribute('class', 'form-group mt-3 mb-3');
    sectionElement.setAttribute('name', `sections`);
    sectionElement.innerHTML = `<label name="Sections[${sectionsIndex}].SectionName"><strong>Section Name</strong></label>
                                    <input name="Sections[${sectionsIndex}].SectionName" class="form-control" placeholder="Section Name">`;

    var addIngredientToSection = document.querySelector('#add-ingredient').cloneNode(true);
    addIngredientToSection.removeAttribute('onclick');
    addIngredientToSection.innerText = 'Add Ingredient'
    var removeIngredientFromSection = document.querySelector('#remove-ingredient').cloneNode(true);

    const AddIngredientRow = function (parentNode, ingredientsIndex) {

        let ingredientRow = document.createElement('li');
        ingredientRow.setAttribute('name', 'ingredients-row');
        ingredientRow.setAttribute('class', 'row mt-3 mb-3');
        ingredientRow.setAttribute('id', 'ingredients-row');
        ingredientRow.innerHTML = `<div class="col-lg-4 col-md-6">
                                        <label><strong>Ingredient Name</strong></label>
                                        <input name="Sections[${sectionsIndex}].Components[${ingredientsIndex}].IngredientName" class="form-control" placeholder="all-purpose flour">
                                        <span asp-validation-for="Sections[${sectionsIndex}].Components[${ingredientsIndex}].IngredientName" class="text-danger"></span>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <label><strong>Ingredient Quantity</strong></label>
                                        <input name="Sections[${sectionsIndex}].Components[${ingredientsIndex}].Quantity" class="form-control" placeholder="200">
                                        <span asp-validation-for="Sections[${sectionsIndex}].Components[${ingredientsIndex}].Quantity" class="text-danger"></span>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <label><strong>Units</strong></label>
                                        <select name="Sections[${sectionsIndex}].Components[${ingredientsIndex}].Unit" class="form-control">
                                            <option>---</option>
                                            <option>gr</option>
                                            <option>kg</option>
                                            <option>ml</option>
                                            <option>l</option>
                                            <option>tbsp</option>
                                            <option>tsp</option>
                                            <option>cup</option>
                                            <option>oz</option>
                                        </select>
                                    </div>`;
        parentNode.insertBefore(ingredientRow, event.target);
        console.log("Added new ingredient line")
        removeIngredientFromSection.classList.remove('disabled');
    };

    addIngredientToSection.addEventListener('click', (event) => {

        event.preventDefault();
        let parent = event.target.parentNode;
        let liElements = parent.querySelectorAll('li');
        let ingredientsIndex = liElements.length;
        AddIngredientRow(parent, ingredientsIndex);
    });

    sectionElement.appendChild(addIngredientToSection);
    sectionElement.appendChild(removeIngredientFromSection);

    const RemoveIngredientRow = function (parentNode) {
        let liElements = parentNode.querySelectorAll('li');
        if (liElements.length > 0) {
            let lastListElement = liElements[liElements.length - 1];
            parentNode.removeChild(lastListElement);
            console.log('Removed last ingredient row');

            if (parentNode.querySelectorAll('li').length == 0) {
                removeIngredientFromSection.classList.add('disabled');
            }
        } else {
            console.log('No elements to remove')
        }

    };
    removeIngredientFromSection.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        RemoveIngredientRow(parent);
    });

    parent.appendChild(sectionElement);
    console.log("Added new section")
    document.querySelector('#remove-section').classList.remove('disabled');
};

document.addEventListener('DOMContentLoaded', () => {
  "use strict";

  /**
   * Preloader
   */
  const preloader = document.querySelector('#preloader');
  if (preloader) {
    window.addEventListener('load', () => {
      preloader.remove();
    });
  }

  /**
   * Sticky header on scroll
   */
  const selectHeader = document.querySelector('#header');
  if (selectHeader) {
    document.addEventListener('scroll', () => {
      window.scrollY > 100 ? selectHeader.classList.add('sticked') : selectHeader.classList.remove('sticked');
    });
  }
    /**
   * Custom
   */

    //Remove Ingredients row
    let removeIngredientButtonElement = document.querySelector('#remove-ingredient'); 
    const RemoveIngredientRow = function (parentNode) {
        let liElements = parentNode.querySelectorAll('li');
        if (liElements.length > 0) {
            let lastListElement = liElements[liElements.length - 1];
            parentNode.removeChild(lastListElement);
            console.log('Removed last ingredient row');
            if (parentNode.querySelectorAll('li').length == 0) {
                removeIngredientButtonElement.classList.add('disabled');
            }
        } else {
            console.log('No elements to remove')
        }
        
    };
    removeIngredientButtonElement.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        RemoveIngredientRow(parent);
    });
 
    //Remove Section
    const RemoveSection = function (parentNode) {
        let sectionElements = parentNode.querySelectorAll("ul[name='sections']");
        if (sectionElements.length > 0) {
            let lastSectionElement = sectionElements[sectionElements.length - 1];
            parentNode.removeChild(lastSectionElement);
            console.log('Removed last section');
            if (parentNode.querySelectorAll("ul[name='sections']").length == 0) {
                document.querySelector('#remove-section').classList.add('disabled');
            }
        } else {
            console.log('No elements to remove')
        }

    };
    document.querySelector('#remove-section').addEventListener('click', (event) =>
    {
        event.preventDefault();
            let parent = event.target.parentNode.parentNode;
            RemoveSection(parent);
    });

    //Remove Nutrients row
    let removeNutrientButtonElement = document.querySelector('#remove-nutrient');
    const RemoveNutrientRow = function (parentNode) {
        let liElements = parentNode.querySelectorAll('li');
        if (liElements.length > 0) {
            let lastListElement = liElements[liElements.length - 1];
            parentNode.removeChild(lastListElement);
            console.log('Removed last nutrient row');
            if (parentNode.querySelectorAll('li').length == 0) {
                removeNutrientButtonElement.classList.add('disabled');
            }
        } else {
            console.log('No elements to remove')
        }

    };
    removeNutrientButtonElement.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        RemoveNutrientRow(parent);
    });


    /**
   * Navbar links active state on scroll
   */
  let navbarlinks = document.querySelectorAll('#navbar a');

  function navbarlinksActive() {
    navbarlinks.forEach(navbarlink => {

      if (!navbarlink.hash) return;

      let section = document.querySelector(navbarlink.hash);
      if (!section) return;

      let position = window.scrollY + 200;

      if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
        navbarlink.classList.add('active');
      } else {
        navbarlink.classList.remove('active');
      }
    })
  }
  window.addEventListener('load', navbarlinksActive);
  document.addEventListener('scroll', navbarlinksActive);

  /**
   * Mobile nav toggle
   */
  const mobileNavShow = document.querySelector('.mobile-nav-show');
  const mobileNavHide = document.querySelector('.mobile-nav-hide');

  document.querySelectorAll('.mobile-nav-toggle').forEach(el => {
    el.addEventListener('click', function(event) {
      event.preventDefault();
      mobileNavToogle();
    })
  });

  function mobileNavToogle() {
    document.querySelector('body').classList.toggle('mobile-nav-active');
    mobileNavShow.classList.toggle('d-none');
    mobileNavHide.classList.toggle('d-none');
  }

  /**
   * Hide mobile nav on same-page/hash links
   */
  document.querySelectorAll('#navbar a').forEach(navbarlink => {

    if (!navbarlink.hash) return;

    let section = document.querySelector(navbarlink.hash);
    if (!section) return;

    navbarlink.addEventListener('click', () => {
      if (document.querySelector('.mobile-nav-active')) {
        mobileNavToogle();
      }
    });

  });

  /**
   * Toggle mobile nav dropdowns
   */
  const navDropdowns = document.querySelectorAll('.navbar .dropdown > a');

  navDropdowns.forEach(el => {
    el.addEventListener('click', function(event) {
      if (document.querySelector('.mobile-nav-active')) {
        event.preventDefault();
        this.classList.toggle('active');
        this.nextElementSibling.classList.toggle('dropdown-active');

        let dropDownIndicator = this.querySelector('.dropdown-indicator');
        dropDownIndicator.classList.toggle('bi-chevron-up');
        dropDownIndicator.classList.toggle('bi-chevron-down');
      }
    })
  });

  /**
   * Scroll top button
   */
  const scrollTop = document.querySelector('.scroll-top');
  if (scrollTop) {
    const togglescrollTop = function() {
      window.scrollY > 100 ? scrollTop.classList.add('active') : scrollTop.classList.remove('active');
    }
    window.addEventListener('load', togglescrollTop);
    document.addEventListener('scroll', togglescrollTop);
    scrollTop.addEventListener('click', window.scrollTo({
      top: 0,
      behavior: 'smooth'
    }));
  }

  /**
   * Initiate glightbox
   */
  const glightbox = GLightbox({
    selector: '.glightbox'
  });

  /**
   * Initiate pURE cOUNTER
   */
  new PureCounter();

  /**
   * Init swiper slider with 1 slide at once in desktop view
   */
  new Swiper('.slides-1', {
    speed: 600,
    loop: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    }
  });

  /**
   * Init swiper slider with 3 slides at once in desktop view
   */
  new Swiper('.slides-3', {
    speed: 600,
    loop: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
    breakpoints: {
      320: {
        slidesPerView: 1,
        spaceBetween: 40
      },

      1200: {
        slidesPerView: 3,
      }
    }
  });

  /**
   * Gallery Slider
   */
  new Swiper('.gallery-slider', {
    speed: 400,
    loop: true,
    centeredSlides: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    breakpoints: {
      320: {
        slidesPerView: 1,
        spaceBetween: 20
      },
      640: {
        slidesPerView: 3,
        spaceBetween: 20
      },
      992: {
        slidesPerView: 5,
        spaceBetween: 20
      }
    }
  });

  /**
   * Animation on scroll function and init
   */
  function aos_init() {
    AOS.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
    });
  }
  window.addEventListener('load', () => {
    aos_init();
  });

});