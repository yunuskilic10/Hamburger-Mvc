using MVCProjectHamburger.Models.Entities.Abstract;

namespace MVCProjectHamburger.Models.Entities.Concrete
{
    public class ExtraIngredient: BaseEntity
    {
        public ExtraIngredient()
        {
            
            OrderExtraIngredients = new List<ExtraIngredientOrder>();  
        }
     
        public ICollection<ExtraIngredientOrder> OrderExtraIngredients { get; set; }
        
    }
}
