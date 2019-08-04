using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RxProp.Models;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxProp.ViewModels
{
    public class ItemDetailViewModel : ReactiveObject
    {
        public Item Item { get; set; }

        [Reactive]
        public string CEP { get; set; }

        public string CEPBusca { [ObservableAsProperty] get; }

        public ItemDetailViewModel(Item item = null)
        {
            Item = item;

            CEP = "";
            this.WhenAnyValue(x => x.CEP)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length == 9)
                .DistinctUntilChanged()
                .Select(x => x.Replace("-", "").Trim())
                .ToPropertyEx(this, e => e.CEPBusca);

            var result = this.WhenAnyValue(x => x.CEPBusca)
                .Select(Write)
                .Switch()
                .Subscribe(x => Debug.WriteLine(x));
        }

        private async Task<string> Write(string x)
        {
            var r = new Random();
            await Task.Delay(r.Next(500, 2000));
            return x;
        }
    }
}
