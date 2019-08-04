using RxProp.Models;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxProp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        string cep = string.Empty;
        public string CEP
        {
            get { return cep; }
            set { SetProperty(ref cep, value); }
        }

        string cepBusca = string.Empty;
        public string CEPBusca
        {
            get { return cepBusca; }
            set { SetProperty(ref cepBusca, value); }
        }

        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;

            CEP = "";
            var obs = this.ToObservable(x => x.CEP)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length == 9)
                .DistinctUntilChanged()
                .Select(x => x.Replace("-", "").Trim());

            obs.Select(Write)
                .Switch()
                .Do(x => Debug.WriteLine(x));

            //CEPBusca = CEP.Throttle(TimeSpan.FromMilliseconds(500))
            //    .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length == 9)
            //    .DistinctUntilChanged()
            //    .Select(x => x.Replace("-", "").Trim())
            //    .ToReadOnlyReactiveProperty();

            //var result = CEPBusca.Select(Write)
            //    .Switch()
            //    .Do(x => Debug.WriteLine(x))
            //    .ToReactiveProperty();
        }

        private async Task<string> Write(string x)
        {
            var r = new Random();
            await Task.Delay(r.Next(500, 2000));
            return x;
        }
    }
}
