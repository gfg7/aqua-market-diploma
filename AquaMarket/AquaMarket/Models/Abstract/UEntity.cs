using AquaServer.Interfaces.Models;
using System;

namespace AquaServer.Models.Abstract
{
    public class UEntity : Base, IEntity
    {
        private string article;

        public string Article { get => article.Trim(); set => article = value.Trim(); }
        public override IComparable GetId()
        {
            return Article;
        }
    }
}
