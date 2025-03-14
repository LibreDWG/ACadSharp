using ACadSharp.Entities;
using ACadSharp.Tests.Common;
using CSMath;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace ACadSharp.Tests.Entities
{
	public class ArcTests
	{
		[Fact]
		public void CreateFromBulgeTest()
		{
			bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
			bool isNetFramework =
#if NETFRAMEWORK
			    true;
#else
			    false;
#endif
			bool needsFixZero = isLinux || isNetFramework;
			XY start = new XY(1, 0);
			XY end = new XY(0, 1);
			// 90 degree bulge
			double bulge = Math.Tan(Math.PI / (2 * 4));

			XY center = Arc.GetCenter(start, end, bulge, out double radius);

			if (needsFixZero)
				center = MathHelper.FixZero(center);

			Assert.Equal(XY.Zero, center);
			Assert.Equal(1, radius, TestVariables.DecimalPrecision);

			Arc arc = Arc.CreateFromBulge(start, end, bulge);

			if (needsFixZero)
				arc.Center = MathHelper.FixZero(arc.Center);

			Assert.Equal(XYZ.Zero, arc.Center);
			Assert.Equal(1, arc.Radius, TestVariables.DecimalPrecision);
			Assert.Equal(0, arc.StartAngle, TestVariables.DecimalPrecision);
			Assert.Equal(Math.PI / 2, arc.EndAngle, TestVariables.DecimalPrecision);
		}

		[Fact]
		public void GetBoundingBoxTest()
		{
			Arc arc = new Arc();
			arc.Radius = 5;
			arc.EndAngle = Math.PI / 2;

			BoundingBox boundingBox = arc.GetBoundingBox();

			Assert.Equal(new XYZ(0, 0, 0), boundingBox.Min);
			Assert.Equal(new XYZ(5, 5, 0), boundingBox.Max);
		}

		[Fact]
		public void GetCenter()
		{
			bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
			bool isNetFramework =
#if NETFRAMEWORK
			    true;
#else
			    false;
#endif
			bool needsFixZero = isLinux || isNetFramework;
			XY start = new XY(1, 0);
			XY end = new XY(0, 1);
			// 90 degree bulge
			double bulge = Math.Tan(Math.PI / (2 * 4));

			XY center = Arc.GetCenter(start, end, bulge);

			if (needsFixZero)
				center = MathHelper.FixZero(center);

			Assert.Equal(XY.Zero, center);

			Arc arc = Arc.CreateFromBulge(start, end, bulge);

			if (needsFixZero)
				arc.Center = MathHelper.FixZero(arc.Center);

			Assert.Equal(XYZ.Zero, arc.Center);
			Assert.Equal(1, arc.Radius, TestVariables.DecimalPrecision);
			Assert.Equal(0, arc.StartAngle, TestVariables.DecimalPrecision);
			Assert.Equal(Math.PI / 2, arc.EndAngle, TestVariables.DecimalPrecision);
		}

		[Fact]
		public void GetEndVerticesTest()
		{
			XY start = new XY(1, 0);
			XY end = new XY(0, 1);
			// 90 degree bulge
			double bulge = Math.Tan(Math.PI / (2 * 4));

			Arc arc = Arc.CreateFromBulge(start, end, bulge);

			arc.GetEndVertices(out XY s1, out XY e2);

			AssertUtils.AreEqual<XY>(start, s1, "start point");
			AssertUtils.AreEqual<XY>(end, e2, "end point");
		}
	}
}
