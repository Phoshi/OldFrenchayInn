using System;
using System.Collections.Generic;
using System.Linq;

namespace OldFrenchayInn {
    public static class CollapseRangesExtension {
        /// <summary>
        /// Takes a list of stringy integers and collapses any ranges found within
        /// Example: [1, 2, 3, 5, 8, 9, 10] => [1-3, 5, 8-10]
        /// </summary>
        /// <param name="self">The list to collapse</param>
        /// <returns>A collapsed list</returns>
        public static IEnumerable<string> CollapseRanges(this IEnumerable<string> self){
            Func<string, bool> isNumeric = (element =>{
                                                int num;
                                                return int.TryParse(element, out num);
                                            });
            if (!self.All(isNumeric)){
                return self;
            }

            var returnList = new List<string>();
            var selfStack = new Stack<int>(self.Select(int.Parse));

            while (selfStack.Count > 0){
                var maxElement = selfStack.Pop();
                var minElement = maxElement;

                while (selfStack.Count > 0 && selfStack.Peek() == minElement - 1){
                    minElement = selfStack.Pop();
                }

                returnList.Add(maxElement != minElement
                                   ? string.Format("{0}-{1}", minElement, maxElement)
                                   : maxElement.ToString());
            }

            returnList.Reverse();
            return returnList;
        } 
    }
}
