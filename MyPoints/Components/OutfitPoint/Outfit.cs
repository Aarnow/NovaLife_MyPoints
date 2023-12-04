using MyCloths.Components;

namespace MyPoints.Components.OutfitPoint
{
    public class Outfit
    {
        public string Name { get; set; }
        public int SexId { get; set; }
        public PCloth Hat { get; set; }
        public PCloth Accessory { get; set; }
        public PCloth Shirt { get; set; }
        public PCloth Pants { get; set; }
        public PCloth Shoes { get; set; }

        public Outfit()
        { 
        }
    }
}
