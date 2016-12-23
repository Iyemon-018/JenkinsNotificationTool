namespace Server.Simulator.Communicators
{
    using System;
    using System.Net;

    /// <summary>
    /// WebSocket �ɂ��ʐM�ŃN���C�A���g�Ƃ̐ڑ��A�ؒf�����o�����Ƃ��̃C�x���g�����N���X�ł��B
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class WebSocketClientEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// �N���C�A���g�̃G���h�|�C���g
        /// </summary>
        private readonly IPEndPoint _clientEndPoint;

        #endregion

        #region Ctor

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="clientEndPoint">�N���C�A���g�̃G���h�|�C���g</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="clientEndPoint"/> ��null �̏ꍇ�ɃX���[����܂��B</exception>
        public WebSocketClientEventArgs(IPEndPoint clientEndPoint)
        {
            if (clientEndPoint == null) throw new ArgumentNullException(nameof(clientEndPoint));
            _clientEndPoint = clientEndPoint;
        }

        #endregion

        #region Properties

        /// <summary>
        /// �N���C�A���g�̃G���h�|�C���g���擾���܂��B
        /// </summary>
        public IPEndPoint ClientEndPoint => _clientEndPoint;

        #endregion
    }
}