using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    public class Room{
        private static List<Room> _rooms = new List<Room>{
                                                             new Room(1, RoomType.Get(RoomTypes.Single)),
                                                             new Room(2, RoomType.Get(RoomTypes.Single)),
                                                             new Room(3, RoomType.Get(RoomTypes.Single)),
                                                             new Room(4, RoomType.Get(RoomTypes.Single)),
                                                             new Room(5, RoomType.Get(RoomTypes.Double)),
                                                             new Room(6, RoomType.Get(RoomTypes.Double)),
                                                             new Room(7, RoomType.Get(RoomTypes.Family)),
                                                             new Room(8, RoomType.Get(RoomTypes.Family)),
                                                         };

        public static IEnumerable<Room> Rooms{
            get { return _rooms.AsReadOnly(); }
        }

        public static Room Get(int number){
            int roomNumber = number - 1;
            if (number < 1 || number > _rooms.Count){
                throw new IndexOutOfRangeException("No such room!");
            }
            return _rooms[roomNumber];
        }


        public RoomType Type { get; internal set; }
        public int Number { get; internal set; }

        public Room(int number, RoomType type){
            Number = number;
            Type = type;
        }

        public override bool Equals(object obj) {
            if (obj is Room){
                Room other = obj as Room;
                return other.Number == Number;
            }
            return base.Equals(obj);
        }

        public override string ToString(){
            return string.Format("Number {0} ({1})", Number, Type);
        }
    }
}
