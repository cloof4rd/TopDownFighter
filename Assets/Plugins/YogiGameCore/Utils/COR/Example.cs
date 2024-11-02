// namespace YogiGameCore.Utils.COR
// {
//     public class Example
//     {
//         public void Main()
//         {
//             // 入参之一
//             Person a = new Person()
//             {
//                 Name = "A",
//                 Age = 10,
//                 Money = 0
//             };
//             // 上下文数据
//             PersonContent personContent = new PersonContent()
//             {
//                 PersonData = a,
//                 Index = 0 //这里随便写的,实际游戏内可能是其他数据 比如如何处理等
//             };
//
//             // 初始化逻辑链路管理器
//             // ChainProcess<PersonContent> chain = new ChainProcess<PersonContent>(); //替代方案,如果不需要重写出入库的话...
//             PersonMoneyModifier chain = new PersonMoneyModifier();
//
//             //复杂逻辑链路构建
//             chain.AddContent(personContent) // Input content
//                 .IF((content) => content.PersonData.Age < 5) // If 可以传入 Func<Content,bool>
//                 .AddLogic((content) => { content.PersonData.Money += 10; }) //Do  可以传入 Action<Content>
//                 .IF(() => true) // Filter可以传入 Func<bool>
//                 .AddLogic(() => { }) // Logic也可以传入 Action
//                 .IF((content) => content.PersonData.Age < 5) // if == and
//                 .And((content) => content.Index == 0) // And
//                 .Or((content) => content.PersonData.Money == 0) // Or
//                 .And(content => content.PersonData.Age == 10 && content.PersonData.Name != "B") // 复杂判断逻辑
//                 .Or(c => c.PersonData.IsYogi()) // 通过调用判断内置方法
//                 .IF(c => c.PersonData.Age < 3)
//                 .Or(c => IsOldMan(c.PersonData, 80)) //可以调用其他方法
//                 .AddLogic(c => c.PersonData.Money += 20)
//                 .Process(); // Do
//         }
//
//         public class Person
//         {
//             public string Name;
//             public int Age;
//             public float Money;
//
//             public bool IsYogi()
//             {
//                 return Name == "Yogi";
//             }
//         }
//
//         public bool IsOldMan(Person person, float OldManAge)
//         {
//             return person.Age > OldManAge;
//         }
//
//         /// <summary>
//         /// 上下文数据
//         /// </summary>
//         public class PersonContent : IContent
//         {
//             public Person PersonData;
//             public float Index;
//         }
//
//         /// <summary>
//         /// 出入库脚本处理器
//         /// </summary>
//         public class PersonMoneyModifier : ChainProcess<PersonContent>
//         {
//             public override void HandleAssetIn(PersonContent content)
//             {
//                 base.HandleAssetIn(content);
//                 content.PersonData.Age += 10;
//             }
//
//             public override void HandleAssetOut(PersonContent content)
//             {
//                 base.HandleAssetOut(content);
//                 content.PersonData.Age -= 10;
//             }
//         }
//     }
// }