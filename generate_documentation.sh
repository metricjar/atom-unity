mkdir doc && doxygen doxyfile

REPO_COMMIT_AUTHOR=$(git show -s --pretty=format:"%cn")
REPO_COMMIT_AUTHOR_EMAIL=$(git show -s --pretty=format:"%ce")

git config --global user.email "$REPO_COMMIT_AUTHOR_EMAIL"
git config --global user.name "$REPO_COMMIT_AUTHOR"

TARGET_BRANCH="gh-pages"
mkdir $TARGET_BRANCH && cd $TARGET_BRANCH 
git clone -b $TARGET_BRANCH --single-branch https://github.com/ironSource/atom-unity.git

cd atom-unity
cp -r ../../doc/* .

git add .
git commit -m "Deploy to GitHub Pages: ${SHA}"

# Now that we're all set up, we can push.
git push origin $TARGET_BRANCH