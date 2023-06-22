public class SectionInputModel
{
    public SectionInputModel()
    {
        this.Components = new List<ComponentInputModel>();
    }
    public int Id { get; set; }
    public string SectionName { get; set; }
    public List<ComponentInputModel> Components { get; set; }
}
