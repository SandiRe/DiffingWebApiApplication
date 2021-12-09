using NUnit.Framework;

namespace DiffingWebApiApplication.Tests.HelperClasses
{
    public class DiffingHelperTests
    {
        [Test]
        public void CompareBase64CodedBinaryValues_LeftValueIsEqualRightValue_CorrectResultIsReturned()
        {
            var result = DiffingHelper.CompareBase64CodedBinaryValues("AAAAAA==", "AAAAAA==");

            Assert.AreEqual(result.DiffingResult, DiffingResultType.Equals);
            Assert.IsNull(result.Differences);
        }

        [Test]
        public void CompareBase64CodedBinaryValues_LeftValueSizeDoesNotMatchRightValueSize_CorrectResultIsReturned()
        {
            var result = DiffingHelper.CompareBase64CodedBinaryValues("AAA=", "AAAAAA==");

            Assert.AreEqual(result.DiffingResult, DiffingResultType.SizeDoNotMatch);
            Assert.IsNull(result.Differences);
        }

        [Test]
        public void CompareBase64CodedBinaryValues_LeftValueContentDoesNotMatchRightValueContent_CorrectResultIsReturned()
        {
            var result = DiffingHelper.CompareBase64CodedBinaryValues("AAAAAA==", "AQABAQ==");

            Assert.AreEqual(result.DiffingResult, DiffingResultType.ContentDoNotMatch);
            Assert.AreEqual(result.Differences.Count, 2);
            Assert.AreEqual(result.Differences[0].Offset, 0);
            Assert.AreEqual(result.Differences[0].Length, 1);
            Assert.AreEqual(result.Differences[1].Offset, 2);
            Assert.AreEqual(result.Differences[1].Length, 2);
        }
    }
}
