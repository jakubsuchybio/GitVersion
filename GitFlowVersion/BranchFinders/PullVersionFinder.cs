namespace GitFlowVersion
{
    using LibGit2Sharp;

    class PullVersionFinder
    {
        public Commit Commit;
        public Repository Repository;
        public Branch PullBranch;

        public SemanticVersion FindVersion()
        {
            var versionFromMaster = Repository
                .MasterBranch()
                .GetVersionPriorTo(Commit.When());

            var versionString = MergeMessageParser.GetVersionFromMergeCommit(versionFromMaster.Version);

            var version = SemanticVersion.FromMajorMinorPatch(versionString);
            version.Minor++;
            version.Patch = 0;
            version.Stage = Stage.Pull;
            version.Suffix = PullBranch.CanonicalName
                .Substring(PullBranch.CanonicalName.IndexOf("/pull/") + 6);
            version.PreRelease = 0;


            return version;
        }
    }
}