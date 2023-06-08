public class SectionInputModel
{
    public SectionInputModel()
    {
        this.Components = new List<ComponentInputModel>();
    }
    public string SectionName { get; set; }
    public List<ComponentInputModel> Components { get; set; }
}
