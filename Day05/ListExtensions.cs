namespace Day05
{
    public static class ListExtensions
    {
        public static IEnumerable<IEnumerable<T>> Permutate<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
                yield break;
            var list = sequence.ToList();
            if (list.Count == 0)
                yield return Enumerable.Empty<T>();
            else
            {
                var start_index = 0;
                foreach (var start_element in list)
                {
                    var index = start_index;
                    var remaining = list.Where((e, i) => i != index);

                    foreach (var perm_of_remainder in Permutate(remaining))
                    {
                        yield return perm_of_remainder.Prepend(start_element);
                    }
                    start_index++;
                }
            }
        }
    }
}
