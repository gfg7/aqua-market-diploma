using System.Reflection;

namespace AquaServer.Extensions.Mappers
{
    public sealed class Map<T, K>
        where T : new()
        where K : new()
    {
        public K From(T source)
        {
            var dist = new K();

            foreach (var item in typeof(K).GetProperties())
            {
                var matchProp = typeof(T).GetProperties().FirstOrDefault(x => x.Name == item.Name && x.GetValue(source) is not null); 

                if (matchProp is not null)
                {
                    item.SetValue(dist, matchProp.GetValue(source));
                }
            }
            return dist;
        }
    }
}
