﻿using MarkSFrancis.Random.Generator.Data.Source;
using MarkSFrancis.Random.Generator.Interfaces;

namespace MarkSFrancis.Random.Generator.Data
{
    public class RandomFirstName : IRandomGenerator<string>
    {
        public string Get()
        {
            return RandomHelper.GetBool() ? GetMale() : GetFemale();
        }

        public string GetMale()
        {
            return RandomHelper.OneOf(Names.MaleFirstNames);
        }

        public string GetFemale()
        {
            return RandomHelper.OneOf(Names.FemaleFirstNames);
        }
    }
}
