namespace DiffingWebApiApplication
{
    public static class DiffingHelper
    {
        public static DiffingResultData CompareBase64CodedBinaryValues(string left, string right)
        {
            if (left is null)
                throw new ArgumentNullException(nameof(left));
            if (right is null)
                throw new ArgumentNullException(nameof(right));

            var diffingResult = new DiffingResultData();

            byte[] leftBytes = Convert.FromBase64String(left);
            byte[] rightBytes = Convert.FromBase64String(right);

            if (leftBytes.Length != rightBytes.Length)
                diffingResult.DiffingResult = DiffingResultIype.SizeDoNotMatch;
            else
            {
                bool diffParsing = false;

                for (int i = 0; i < leftBytes.Length; i++)
                {
                    if (leftBytes[i] != rightBytes[i])
                    {
                        if (!diffParsing)
                        {
                            diffParsing = true;

                            if (diffingResult.Differences == null)
                                diffingResult.Differences = new List<Difference>();

                            diffingResult.Differences.Add(new Difference { Offset = i, Length = 1 });

                            continue;
                        }

                        diffingResult.Differences.Last().Length++;
                        continue;
                    }

                    diffParsing = false;
                }

                if (diffingResult.Differences == null)
                    diffingResult.DiffingResult = DiffingResultIype.Equals;
                else
                    diffingResult.DiffingResult = DiffingResultIype.ContentDoNotMatch;
            }

            return diffingResult;
        }
    }
}
