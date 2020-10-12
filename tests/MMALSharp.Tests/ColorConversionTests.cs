using System.Drawing;
using MMALSharp.Utility;
using Xunit;

namespace MMALSharp.Tests
{
    public class ColorConversionTests
    {        
        [Fact]
        [MMALTestsAttribute]
        public void FromCie1960()
        {          
            var (cu, cv, y) = MmalColor.RgbToCie1960(Color.Blue);
            var from1960 = MmalColor.FromCie1960(cu, cv, y);
            
            Assert.True(from1960.R == Color.Blue.R && from1960.G == Color.Blue.G && from1960.B == Color.Blue.B);
        }
             
        [Fact]
        [MMALTestsAttribute]
        public void FromCiexyz()
        {           
            var (x, y, z) = MmalColor.RgbToCieXyz(Color.Blue);
            var fromXyz = MmalColor.FromCieXyz(x, y, z);

            Assert.True(fromXyz.R == Color.Blue.R && fromXyz.G == Color.Blue.G && fromXyz.B == Color.Blue.B);
        }
                
        [Fact]
        [MMALTestsAttribute]
        public void FromYiq()
        {           
            var (y, i, q) = MmalColor.RgbToYiq(Color.Blue);
            var fromYiq = MmalColor.FromYiq(y, i, q);

            Assert.True(fromYiq.R == Color.Blue.R && fromYiq.G == Color.Blue.G && fromYiq.B == Color.Blue.B);
        }
                
        [Fact]
        [MMALTestsAttribute]
        public void FromYuv()
        {
            var fromYuvBytes = MmalColor.FromYuvBytes(0, 20, 20);
            var (y, u, v) = MmalColor.RgbToYuv(fromYuvBytes);
            var fromYuv = MmalColor.FromYuv(y, u, v);
                        
            Assert.True(fromYuv.Equals(fromYuvBytes));
        }

        [Fact]
        [MMALTestsAttribute]
        public void RgbtoYuvBytes()
        {
            var (y, u, v) = MmalColor.RgbToYuvBytes(Color.Blue);
            var fromYuvBytes = MmalColor.FromYuvBytes(y, u, v);

            Assert.True(fromYuvBytes.R == Color.Blue.R && fromYuvBytes.G == Color.Blue.G && fromYuvBytes.B == Color.Blue.B);
        }

        [Fact]
        [MMALTestsAttribute]
        public void FromHls()
        {            
            var (h, l, s) = MmalColor.RgbToHls(Color.Blue);
            var fromHls = MmalColor.FromHls(h, l, s);

            Assert.True(fromHls.R == Color.Blue.R && fromHls.G == Color.Blue.G && fromHls.B == Color.Blue.B);
        }
                
        [Fact]
        [MMALTestsAttribute]
        public void FromHsv()
        {            
            var (h, s, v) = MmalColor.RgbToHsv(Color.Blue);
            var fromHsv = MmalColor.FromHsv(h, s, v);
            
            Assert.True(fromHsv.R == Color.Blue.R && fromHsv.G == Color.Blue.G && fromHsv.B == Color.Blue.B);
        }        
    }
}
