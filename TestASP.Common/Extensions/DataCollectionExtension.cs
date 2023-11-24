

namespace TestASP.Common.Extensions
{
    public static class DataCollectionExtension
    {
        public static void ForEach<T>(this IEnumerable<T> data, Action<int, T> action)
        {
            int index = 0;
            List<T> dataTemp = data.ToList();
            dataTemp.ForEach(item =>
            {
                action?.Invoke(index, dataTemp[index++]);
                //index++;
            });
        }

        public static void For<T>(this int maxNumber, Action<int> action, int startIndex = 0)
        {
            for(int index = startIndex; index < maxNumber; index ++)
            {
                action?.Invoke(index);
            }
        }


        public static IEnumerable<T> Select<T>(this int maxNumber, Func<int,T> action, int startIndex = 0)
        {
            List<T> data = new List<T>();
            for (int index = startIndex; index < maxNumber; index++)
            {
                T item = action.Invoke(index);
                if (item != null)
                {
                    data.Add(item);
                    //yield return item;
                }
            }

            return data;
        }

        

    }
}

