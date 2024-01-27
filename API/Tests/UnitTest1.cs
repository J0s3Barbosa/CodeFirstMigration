namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //arrange
            var arr = new[] { 1, 2, 3 };
            //act
            //assert
            Assert.Equal(3, arr.Length);
        }
    }
}