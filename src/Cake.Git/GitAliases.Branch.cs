using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Git.Extensions;
// ReSharper disable UnusedMember.Global

namespace Cake.Git
{
    /// <summary>
    /// Class GitAliases.
    /// </summary>
    public static partial class GitAliases
    {
        /// <summary>
        /// Gets the current branch.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <returns>GitBranch.</returns>
        /// <exception cref="ArgumentNullException">context
        /// or
        /// repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitBranchCurrent(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitBranchCurrent(
            this ICakeContext context,
            DirectoryPath repositoryDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (repositoryDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(repositoryDirectoryPath));
            }

            return context.UseRepository(
                repositoryDirectoryPath,
                repository => new GitBranch(repository)
                );
        }

        /// <summary>
        /// Creates a new branch with default settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <param name="branchName">The branch's name</param>
        /// <returns>GitBranch which just got created</returns>
        /// <exception cref="ArgumentNullException">context or repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitCreateBranch(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitCreateBranch(
            this ICakeContext context,
            DirectoryPath repositoryDirectoryPath,
            string branchName)
        {
            var defaultSettings = new GitBranchSettings();
            return GitCreateBranch(context, repositoryDirectoryPath, branchName, defaultSettings);
        }

        /// <summary>
        /// Creates a new branch with the given settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <param name="branchName">The branch's name</param>
        /// <returns>GitBranch which just got created</returns>
        /// <exception cref="ArgumentNullException">context or repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitCreateBranch(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitCreateBranch(
            this ICakeContext context,
            DirectoryPath repositoryDirectoryPath,
            string branchName,
            GitBranchSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (repositoryDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(repositoryDirectoryPath));
            }

            return context.UseRepository(
                repositoryDirectoryPath,
                repository => {
                    var head = repository.Head.Tip;

                    var newBranch = repository
                        .Branches
                        .Add(branchName, head, settings.AllowOverwrite);

                    if (settings.TrackOnRemote)
                    {
                        repository
                            .Branches
                            .Update(newBranch, updater =>
                            {
                                updater.TrackedBranch = $"refs/remotes/{settings.Remote.Name}/{settings.Remote.BranchName ?? branchName}";
                            });
                    }                    

                    return new GitBranch(repository);
                });
        }
    }
}
