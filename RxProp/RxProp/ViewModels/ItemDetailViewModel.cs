using ReactiveUI;
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

        private string cep;

        public string CEP
        {
            get { return cep; }
            set { this.RaiseAndSetIfChanged(ref cep, value); }
        }

        ObservableAsPropertyHelper<string> cepBusca;

        public string CEPBusca
        {
            get { return cepBusca.Value; }
        }

        public ItemDetailViewModel(Item item = null)
        {
            Item = item;

            CEP = "";
            cepBusca = this.WhenAnyValue(x => x.CEP)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length == 9)
                .DistinctUntilChanged()
                .Select(x => x.Replace("-", "").Trim())
                .ToProperty(this, e => e.CEPBusca);

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
