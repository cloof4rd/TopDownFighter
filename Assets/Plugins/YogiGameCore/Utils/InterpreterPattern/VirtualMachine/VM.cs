// /*
//  * 这里仅供参考 使用时机基本上只有在手搓蓝图这类工具的时候使用
//  */
//
// public enum Instruction
// {
//     /// <summary>
//     /// 数据标识
//     /// </summary>
//     LITERAL = 0x00,
//     INST_SET_HEALTH = 0x01,
//     INST_SET_MANA = 0x02,
//     INST_SET_STAMINA = 0x03,
//     INST_PLAY_SOUND = 0x04,
//     INST_SPAWN_PARTICLES = 0x05,
// }
// using System;
// using System.Collections.Generic;
//
// namespace YogiGameCore.Utils.InterpreterPattern.VirtualMachine
// {
//     public class InputContext
//     {
//         public string Order;
//         public int Size;
//     }
//
//     public static class GameFunc
//     {
//         public static void SetHealth(int playerIndex, int value)
//         {
//             //...
//         }
//
//         public static void SetMana(int playerIndex, int value)
//         {
//             //...
//         }
//
//         public static void SetStamina(int playerIndex, int value)
//         {
//             //...
//         }
//
//         public static void PlaySound(int soundIndex)
//         {
//             //...
//         }
//
//         public static void SpawnParticles(int particleIndex)
//         {
//             //...
//         }
//     }
//
//     public class VM : IExpression<InputContext, bool>
//     {
//         private Stack<int> m_Stack = new Stack<int>();
//
//         public bool Interpret(InputContext context)
//         {
//             for (var index = 0; index < context.Size; index++)
//             {
//                 var instruction = context.Order[index];
//                 var inst = (Instruction)instruction;
//                 switch (inst)
//                 {
//                     case Instruction.LITERAL:
//                         var value = context.Order[++index];
//                         m_Stack.Push(value);
//                         break;
//                     case Instruction.INST_SET_HEALTH:
//                         var amount = m_Stack.Pop();
//                         var player = m_Stack.Pop();
//                         GameFunc.SetHealth(player, amount);
//                         break;
//                     case Instruction.INST_SET_MANA:
//                         var amount2 = m_Stack.Pop();
//                         var player2 = m_Stack.Pop();
//                         GameFunc.SetMana(player2, amount2);
//                         break;
//                     case Instruction.INST_SET_STAMINA:
//                         var amount3 = m_Stack.Pop();
//                         var player3 = m_Stack.Pop();
//                         GameFunc.SetStamina(player3, amount3);
//                         break;
//                     case Instruction.INST_PLAY_SOUND:
//                         GameFunc.PlaySound(m_Stack.Pop());
//                         break;
//                     case Instruction.INST_SPAWN_PARTICLES:
//                         GameFunc.SpawnParticles(m_Stack.Pop());
//                         break;
//                     default:
//                         throw new ArgumentOutOfRangeException();
//                 }
//             }
//
//             return true;
//         }
//     }
// }