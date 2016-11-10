using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryRage;

namespace BinaryRage.UnitTests
{
  using System.IO.Compression;

  public class DBTests
    {
        [TestFixture]
        public class InsertTests
        {
            [Test, Sequential]
            public void ShouldInsertAnObjectToStoreWithAnyCompressionLevel(
                  [Values(CompressionLevel.Fastest, CompressionLevel.NoCompression, CompressionLevel.Optimal)] CompressionLevel compressionLevel)
            { 
                var model = new Model{Title ="title1", ThumbUrl="http://thumb.com/title1.jpg", Description="description1", Price=5.0F};

                BinaryRage.DB.Insert<Model>("myModel", model, "dbfile", compressionLevel);

                var result = BinaryRage.DB.Get<Model>("myModel", "dbfile");
               
                Assert.AreEqual(model, result);
                BinaryRage.DB.WaitForCompletion();
                BinaryRage.DB.Remove("myModel", "dbfile");
            }

            [Test]
            public void ShouldInsertAnObjectToStore()
            {
              var model = new Model { Title = "title1", ThumbUrl = "http://thumb.com/title1.jpg", Description = "description1", Price = 5.0F };

              BinaryRage.DB.Insert<Model>("myModel", model, "dbfile");

              var result = BinaryRage.DB.Get<Model>("myModel", "dbfile");

              Assert.AreEqual(model, result);
              BinaryRage.DB.WaitForCompletion();
              BinaryRage.DB.Remove("myModel", "dbfile");
            }

      [Test]
            public void ShouldInsertAListOfObjectsToStore()
            {
                var models = new List<Model> { 
                    new Model{Title ="title1", ThumbUrl="http://thumb.com/title1.jpg", Description="description1", Price=5.0F},
                    new Model{Title ="title2", ThumbUrl="http://thumb.com/title2.jpg", Description="description2", Price=6.0F},
                    new Model{Title ="title3", ThumbUrl="http://thumb.com/title3.jpg", Description="description3", Price=7.0F},
                };
                BinaryRage.DB.Insert<List<Model>>("myModels", models, "dbfile");

                var result = BinaryRage.DB.Get<List<Model>>("myModels", "dbfile");

                CollectionAssert.AreEqual(models, result);
                BinaryRage.DB.WaitForCompletion();
                BinaryRage.DB.Remove("myModels", "dbfile");
            }
        }
    }
}
