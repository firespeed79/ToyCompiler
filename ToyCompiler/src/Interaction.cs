﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyCompiler
{
    //C#和js交互
    static class Interaction
    {
        public static FunStat Print()
        {
            FunStat print = new FunStat();
            print.mFunID = new Token() { tokenType = TokenType.TTID, desc = "print" };
            print.mParams = new List<Token>
            {
                new Token(){ tokenType = TokenType.TTID, desc = "arg1"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg2"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg3"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg4"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg5"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg6"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg7"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg8"},
                new Token(){ tokenType = TokenType.TTID, desc = "arg9"},
            };
            print.mInnerBuild = true;
            print.mInnerAction = () =>
            {
                foreach (Token t in print.mParams)
                {
                    var v = Env.LocalScope.GetVariant(t.desc);
                    if (v != null)
                    {
                        Console.Write($"{v}\t");
                    }
                    else
                    {
                        break;
                    }
                }
                Console.WriteLine();
            };
            print.mInnerVisit = (List<Instruction> code) =>
            {
                code.Add(new Instruction(OpCode.Print));
            };

            return print;
        }

        public static FunStat Len()
        {
            FunStat len = new FunStat();
            len.mFunID = new Token() { tokenType = TokenType.TTID, desc = "len" };
            len.mParams = new List<Token>
            {
                new Token(){tokenType = TokenType.TTID, desc = "arg1"}
            };
            len.mInnerBuild = true;
            len.mInnerAction = () =>
            {
                var v = Env.LocalScope.GetVariant("arg1");
                VArray arr = v.arr;
                Variant r = new Variant();
                r.variantType = VariantType.Number;
                r.num = arr.Length();
                Env.RunStack.Add(r);
            };
            len.mInnerVisit = (List<Instruction> code) =>
            {
                code.Add(new Instruction(OpCode.Len));
            };
            return len;
        }

        public static int fib(int n)
        {
            if (n < 0) return 0;
            if (n < 3) return n;
            return fib(n - 1) + fib(n - 2);
        }

        public static int JsCallCs_Fibonacci(VM vm)
        {
            //从栈上获取参数
            int n = (int)vm.API_ToNumber(0);
            int r = fib(n);
            vm.API_PushNumber(r);
            return 1;//返回值数量
        }

        public static void CsCallJs_Fibonacci(VM vm)
        {
            vm.API_PushNumber(10);
            vm.API_Call("fib");
            int ret = (int)vm.API_ToNumber(0);
            Console.WriteLine(ret);
        }
    }
}