// using System;
// using UniRx;
//
// namespace StateMachines.KeyLogger
// {
//     public static class RxExtensions
//     {
//         public static IObservable<T> FromMyEvent<T>(this ReactiveCollection<T> src)
//         {
//             return Observable.Create<T>((obs) =>
//             {
//                 Action eh = () => obs.OnNext(src);
//                 src.ValueChanged += eh;
//                 return () => src.ValueChanged -= eh;
//             });
//         }
//     }
// }