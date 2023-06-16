using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using Elfie.Serialization;
using System.Linq.Expressions;

namespace OurRecipes.Models.Profiles
{
    internal class IntConverter : IValueConverter<string, int>
    {
        public int Convert(string sourceMember, ResolutionContext context)
        {
            bool isParsable = int.TryParse(sourceMember, out int destination);
            destination = isParsable ? destination : 0;
            return destination;
        }
    }
}

