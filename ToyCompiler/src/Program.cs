﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ToyCompiler
{
    class Program
    {
        //tc file.js vm/tr
        static void Main(string[] args)
        {
            string runtype = args[0];
            string filename = args[1];
            bool debug = false;
            if(args.Length> 2)
            {
                debug = args[2] == "d";
            }

            string script = File.ReadAllText(filename);

            //词法分析
            Lexer lexer = new Lexer();
            if (!lexer.ParseToken(script))
            {
                Console.WriteLine("lexer parse failed!");
                Console.WriteLine(lexer.ShowTokens());
                return;
            }
            Console.WriteLine("lexer parse success!");

            //语法分析
            TokenReader tokenReader = new TokenReader(lexer.mTokenList);
            StatTree tree = new StatTree();
            if (!tree.Parse(tokenReader))
            {
                Console.WriteLine("stat parse failed!");
                return;
            }
            Console.WriteLine("stat parse success!");

            //执行
            if(runtype == "tr")
            {
                //直接解释执行
                Env.RegisterBuildinFunctions();
                tree.Exec(Env.GlobalScope);
            }
            else if(runtype == "vm")
            {
                //编译成指令执行
                VM vm = new VM();
                vm.Visit(tree);
                vm.DumpInstructions();
                vm.AttachDebugger();
                vm.Run();
            }
            Console.WriteLine("press any key to exit...");
            Console.Read();
        }
    }



}
