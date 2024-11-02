// using Codice.Client.Common;
//
// namespace YogiGameCore.Utils.CommandPattern
// {
//     public class CommandPatternTest
//     {
//         public class Player
//         {
//             public int HP;
//         }
//         public class AddOne : ICommand<Player>
//         {
//             public void Execute(Player source)
//             {
//                 source.HP += 1;
//             }
//
//             public void Undo(Player source)
//             {
//                 source.HP -= 1;
//             }
//         }
//         public class MultiTwo : ICommand<Player>
//         {
//             public void Execute(Player source)
//             {
//                 source.HP *= 2;
//             }
//
//             public void Undo(Player source)
//             {
//                 source.HP /= 2;
//             }
//         }
//
//         public static void Main()
//         {
//             Player source = new Player() { HP = 3 };
//             CommandBroker<Player> commandBroker = new CommandBroker<Player>(source);
//             commandBroker.Execute(new AddOne());
//             commandBroker.Execute(new MultiTwo());
//             commandBroker.Undo();
//             commandBroker.Redo();
//         }
//     }
// }