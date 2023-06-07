public class SectionInputModel
{
    public SectionInputModel()
    {
        this.Components = new HashSet<ComponentInputModel>();
    }
    public string SectionName { get; set; }
    public ICollection<ComponentInputModel> Components { get; set; }
}
