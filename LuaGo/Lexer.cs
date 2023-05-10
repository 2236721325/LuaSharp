namespace LuaGo
{
    class Lexer
    {

        /// <summary>
        /// 源代码
        /// </summary>
        public string Chunk { get; set; }
        public string ChunkName{get; set; }
        /// <summary>
        /// 当前行号
        /// </summary>
        public int Line { get; set; }

        private int currentIndex = 0;

        public Lexer(string chunkName, string chunk)
        {
            ChunkName = chunkName;
            Chunk = chunk;
            Line = 1;
        }


        
        public NextToken()
        {
            switch (switch_on)
            {
                default:
            }
        }

        private char next()
        {
            return Chunk[currentIndex++];
        }
      



      
    }
}