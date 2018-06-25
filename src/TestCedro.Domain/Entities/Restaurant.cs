using System;

namespace TestCedro.Domain.Entities
{
    public class Restaurant
    {
        public Guid RestaurantId { get; set; }
        public string Name { get; set; }

        public void Update(Restaurant restaurant)
        {
            Name = restaurant.Name;
        }
    }
}