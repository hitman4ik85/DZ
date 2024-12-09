using FoodPlacesApp.Models;

namespace FoodPlacesApp.Data;

public class FoodPlaceDatabase : IDatabase<FoodPlace>
{
    private readonly List<FoodPlace> _foodPlaces;

    public FoodPlaceDatabase()
    {
        _foodPlaces = new List<FoodPlace>();
    }

    public void Add(FoodPlace item)
    {
        _foodPlaces.Add(item);
    }

    public IEnumerable<FoodPlace> Get()
    {
        return _foodPlaces;
    }

    public void Remove(FoodPlace item)
    {
        _foodPlaces.RemoveAll(fp => fp.Id == item.Id);
    }

    public void Update(FoodPlace oldItem, FoodPlace newItem)
    {
        int index = _foodPlaces.FindIndex(fp => fp.Id == oldItem.Id);
        _foodPlaces[index] = newItem;
    }
}
