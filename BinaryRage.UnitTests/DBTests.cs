using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryRage;
using FizzWare.NBuilder;

namespace BinaryRage.UnitTests
{
    public class DBTests
    {
        [TestFixture]
        public class InsertTests
        {
            [Test]
            public void ShouldInsertAnObjectToStore()
            { 
                var model = new Model{Title ="title1", ThumbUrl="http://thumb.com/title1.jpg", Description="description1", Price=5.0F};
                BinaryRage.DB<Model>.Insert("myModel", model, "dbfile");

                var result = BinaryRage.DB<Model>.Get("myModel", "dbfile");
               
                Assert.AreEqual(model, result);
                BinaryRage.DB<Model>.Remove("myModel", "dbfile");
            }

            [Test]
            public void ShouldInsertAListOfObjectsToStore()
            {
                var models = Builder<Model>.CreateListOfSize(3)
                                .Build().ToList();
                BinaryRage.DB<List<Model>>.Insert("myModels", models, "dbfile");

                var result = BinaryRage.DB<List<Model>>.Get("myModels", "dbfile");

                CollectionAssert.AreEqual(models, result);
                BinaryRage.DB<List<Model>>.Remove("myModels", "dbfile");
            }
        }
    }
}
