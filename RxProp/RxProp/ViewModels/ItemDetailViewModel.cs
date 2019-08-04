using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings;
using RxProp.Models;

namespace RxProp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ReactiveProperty<string> CEP { get; }
        public ReadOnlyReactiveProperty<string> CEPBusca { get; }

        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;

            CEP = new ReactiveProperty<string>("");
            CEPBusca = CEP.Throttle(TimeSpan.FromMilliseconds(500))
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length == 9)
                .DistinctUntilChanged()
                .Select(x => x.Replace("-", "").Trim())
                .ToReadOnlyReactiveProperty();

            var result = CEPBusca.Select(Write)
                .Switch()
                .Do(x => Debug.WriteLine(x))
                .ToReactiveProperty();
        }

        private async Task<string> Write(string x)
        {
            var r = new Random();
            await Task.Delay(r.Next(500, 2000));
            return x;
        }
    }
}
