﻿using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class LevelUpCommand : AbstractCommand<LevelUpCommandOption>
    {
        public override string Name => "LevelUp";
        public override string Description => "Undergoes leveling up process";

        public LevelUpCommand(Parser parser) : base(parser)
        {
            Aliases.Add("LvlUp");
            Aliases.Add("LVLUP");
        }

        protected override async Task Execute(FieldUser sender, LevelUpCommandOption option)
        {
            if (option.Value < 0)
            {
                await sender.Message($"ERROR: this command cannot de-level character use only positive integers!");
                return;
            }

            await sender.ModifyStats(s =>
            {
                for (int i = 0; i < option.Value; i++)
                {
                    s.LevelUp();
                }
            });

            await sender.Message($"Successfully underwent level up {option.Value} times!");
        }
    }

    public class LevelUpCommandOption
    {
        [Value(0, MetaName = "value", HelpText = "The number of times character will level up.", Required = true)]
        public int Value { get; set; }
    }
}