using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace RxProp
{
    public static class NotifyPropertyChangeReactiveExtensions
    {
        public static IObservable<R> ToObservable<T, R>(this T target, Expression<Func<T, R>> property) where T : INotifyPropertyChanged
        {
            var f = (property as LambdaExpression).Body as MemberExpression;

            if (f == null)
                throw new NotSupportedException("Only use expressions that call a single property");

            var propertyName = f.Member.Name;
            var getValueFunc = property.Compile();
            return Observable.Create<R>(o =>
            {
                PropertyChangedEventHandler eventHandler = new PropertyChangedEventHandler((s, pce) =>
                {
                    if (pce.PropertyName == null || pce.PropertyName == propertyName)
                        o.OnNext(getValueFunc(target));
                });
                target.PropertyChanged += eventHandler;
                return () => target.PropertyChanged -= eventHandler;
            });
        }
    }
}
