using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    public class RoomType {
        public static RoomType Get(RoomTypes type){
            switch (type){
                case RoomTypes.Single:
                    return new RoomType("Single", 30, new[]{
                                                                 RoomFeature.Get(RoomFeatures.Internet),
                                                                 RoomFeature.Get(RoomFeatures.Minibar)
                                                             });
                case RoomTypes.Double:
                    return new RoomType("Double", 40, new[]{
                                                                 RoomFeature.Get(RoomFeatures.Minibar),
                                                                 RoomFeature.Get(RoomFeatures.TV)
                                                             });
                case RoomTypes.Family:
                    return new RoomType("Family", 50, new[]{
                                                                 RoomFeature.Get(RoomFeatures.Internet),
                                                                 RoomFeature.Get(RoomFeatures.TV)
                                                             });
                default:
                    return Get(RoomTypes.Single);

            }
        }


        public string Name { get; internal set; }
        public double Price { get; internal set; }

        private List<RoomFeature> _features;
        public IEnumerable<RoomFeature> Features{
            get { return _features.AsReadOnly(); } 
            internal set { _features = value.ToList(); }
        }

        public RoomType(string name, double price, IEnumerable<RoomFeature> features = null){
            Name = name;
            Price = price;
            Features = features ?? new List<RoomFeature>();
        }

        public override bool Equals(object obj) {
            if (obj is RoomType){
                RoomType other = obj as RoomType;
                return other.Name == Name;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode(){
            return Name.GetHashCode();
        }

        public override string ToString(){
            string features =
                "\t" + string.Join("\n\t",
                            from feature in _features
                            select string.Format("{0}:\n\t\t{1}", feature.Name, feature.Description));
            return string.Format("{0}\n{1}", Name, features);
        }
    }
}
