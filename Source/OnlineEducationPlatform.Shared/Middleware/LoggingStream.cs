using Microsoft.Extensions.Logging;

namespace OnlineEducationPlatform.Shared.Middleware
{
    public class LoggingStream : Stream
    {
        private readonly Stream _originalStream;
        private readonly ILogger _logger;
        private bool _hasBeenRead = false;

        public LoggingStream(Stream originalStream, ILogger logger)
        {
            _originalStream = originalStream;
            _logger = logger;
        }

        public override bool CanRead => _originalStream.CanRead;
        public override bool CanSeek => _originalStream.CanSeek;
        public override bool CanWrite => _originalStream.CanWrite;
        public override long Length => _originalStream.Length;

        public override long Position
        {
            get => _originalStream.Position;
            set => _originalStream.Position = value;
        }

        public override void Flush() => _originalStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!_hasBeenRead)
            {
                _logger.LogWarning("The request body is being read before model binding. This may affect model binding.");
                _hasBeenRead = true;
            }
            return _originalStream.Read(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (!_hasBeenRead)
            {
                _logger.LogWarning("The request body is being read before model binding. This may affect model binding.");
                _hasBeenRead = true;
            }
            return await _originalStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin) => _originalStream.Seek(offset, origin);

        public override void SetLength(long value) => _originalStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => _originalStream.Write(buffer, offset, count);
    }
}