using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BlockchainExample
{
    public class Block
    {
        private int _index;
        private DateTime _timestamp;
        private object _data;
        private string _previousHash;
        private string _hash;
        private int _nonce;
        private readonly int _difficulty;

        public Block(int index, DateTime timestamp, object data, string previousHash, int difficulty)
        {
            _index = index;
            _timestamp = timestamp;
            _data = data;
            _previousHash = previousHash;
            _difficulty = difficulty;
            _nonce = 0;
            _hash = CalculateHash();
        }

        public int Index
        {
            get { return _index; }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        public object Data
        {
            get { return _data; }

        }

        public string PreviousHash
        {
            get { return _previousHash; }
        }

        public string Hash
        {
            get { return _hash; }
        }

        public int Nonce
        {
            get { return _nonce; }
        }

        public void MineBlock(int difficulty)
        {
            string target = new string('0', difficulty);
            while (_hash.Substring(0, difficulty) != target)
            {
                _nonce++;
                _hash = CalculateHash();
            }
        }

        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(_index.ToString() + _timestamp.ToString() + _data + _previousHash + _nonce);
                byte[] outputBytes = sha256.ComputeHash(inputBytes);

                return BitConverter.ToString(outputBytes).Replace("-", "");
            }
        }

        public bool IsValid()
        {
            return _hash == CalculateHash() && _hash.Substring(0, _difficulty) == new string('0', _difficulty);
        }
        public override string ToString()
        {
            return $"Block: Index = {Index}, TimeStamp = {Timestamp}, PreviousHash = {PreviousHash}, Hash = {Hash}, Data = {Data}";
        }

    }

    public class Blockchain : IEnumerable<Block>
    {
        private readonly List<Block> _chain;
        private readonly int _difficulty;

        public Blockchain(int difficulty)
        {
            _chain = new List<Block>();
            _difficulty = difficulty;
            _chain.Add(CreateGenesisBlock());
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return _chain.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _chain.GetEnumerator();
        }

        public Block CreateGenesisBlock()
        {
            return new Block(0, DateTime.Now, "Genesis Block", "0", _difficulty);
        }

        public Block GetLatestBlock()
        {
            return _chain[_chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            block.PreviousHash = GetLatestBlock().Hash;
            block.MineBlock(_difficulty);
            _chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < _chain.Count; i++)
            {
                Block currentBlock = _chain[i];
                Block previousBlock = _chain[i - 1];

                if (!currentBlock.IsValid() || currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
