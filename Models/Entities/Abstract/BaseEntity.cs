namespace MVCProjectHamburger.Models.Entities.Abstract
{
    public class BaseEntity:BaseRelation
    { 
        public string Name { get; set; }
        public int Price { get; set; }
        public string CoverImage { get; set; }

    }
}
