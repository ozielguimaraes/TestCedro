using System;

namespace TestCedro.Domain.Entities
{
    public class Dish
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public void Update(Dish dish)
        {
            Name = dish.Name;
            Value = dish.Value;
            RestaurantId = dish.RestaurantId;
        }
    }
}