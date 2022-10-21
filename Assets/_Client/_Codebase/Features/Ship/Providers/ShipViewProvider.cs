using System.Threading.Tasks;

namespace _Client
{
    public class ShipViewProvider : LocalAssetLoader
    {
        public async Task<ShipView> Load()
        {
            var view = await Load<ShipView>("ShipView");

            return view;
        }
    }
}