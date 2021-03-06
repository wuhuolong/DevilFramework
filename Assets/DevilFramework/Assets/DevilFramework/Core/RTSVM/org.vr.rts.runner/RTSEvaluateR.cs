namespace org.vr.rts.runner
{

    public class RTSEvaluateR : IRTSRunner
    {

        private IRTSLinker mLeftL;
        private IRTSLinker mRightL;
        private IRTSRunner mLeft;
        private IRTSRunner mRight;
        private org.vr.rts.linker.RTSBinaryL mPreRunner;
        private bool mChildLoaded;

        public RTSEvaluateR(IRTSLinker left, IRTSLinker right, org.vr.rts.linker.RTSBinaryL preRunner)
        {
            mLeftL = left;
            mRightL = right;
            mPreRunner = preRunner;
        }

        public IRTSDefine.Stack applyStack()
        {
            return 0;
        }

        public bool isConst()
        {
            return false;
        }

        public void loadedOnThread()
        {
            mChildLoaded = false;
            mLeft = null;
            mRight = null;
        }

        public bool onReturnAndSkip(IRTSDefine.Stack returnTppe, object value)
        {
            return true;
        }

        public object getOutput()
        {
            return mLeft == null ? null : mLeft.getOutput();
        }

        public IRTSDefine.Stack run(IRTSStack stack)
        {
            if (mLeftL == null)
                return 0;
            if (!mChildLoaded)
            {
                mChildLoaded = true;
                bool ret = false;
                if (mPreRunner != null)
                {
                    mRight = mPreRunner.createRunnerWith(mLeftL, mRightL);
                }
                else
                {
                    mRight = mRightL == null ? null : mRightL.createRunner();
                    mLeft = mLeftL.createRunner();
                    if (mLeftL.getId() != IRTSDefine.Linker.VARIABLE)
                        ret = stack.getThread().loadRunner(mLeft);
                }
                ret |= stack.getThread().loadRunner(mRight);
                if (ret)
                    return 0;
            }
            if (mPreRunner != null && mRight != null)
            {
                mLeft = ((RTSBinaryR)mRight).getLeft();
            }
            if (mLeft != null)
            {
                mLeft.evaluate(stack, mRight == null ? null : mRight.getOutput());
            }
            return 0;
        }

        public bool evaluate(IRTSStack stack, object value)
        {
            return false;
        }
    }
}
