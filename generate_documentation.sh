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
# Get the deploy key by using Travis's stored variables to decrypt deploy_key.enc
#ENCRYPTED_KEY_VAR="encrypted_${ENCRYPTION_LABEL}_key"
#ENCRYPTED_IV_VAR="encrypted_${ENCRYPTION_LABEL}_iv"
#ENCRYPTED_KEY=${!ENCRYPTED_KEY_VAR}
#ENCRYPTED_IV=${!ENCRYPTED_IV_VAR}
#openssl aes-256-cbc -K $ENCRYPTED_KEY -iv $ENCRYPTED_IV -in deploy_key.enc -out deploy_key -d

# Now that we're all set up, we can push.
git push origin $TARGET_BRANCH