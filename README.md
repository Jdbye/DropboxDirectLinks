# Dropbox Direct Links
Simple concept: Change dropbox links copied to clipboard automatically to direct links. Dropbox allows us to do this thanks to the URI parameter raw=1
We only want this for files, not folders. raw=1 makes folders download as a zip file. We don't want this. Usually.
In my testing, these are always folders:
https://www.dropbox.com/scl/fo/
https://www.dropbox.com/sh/
hese are always files:
https://www.dropbox.com/scl/fi/
https://www.dropbox.com/s/

It makes sense that "fo" means folder, and "fi" means file. As for the others, who knows what goes.
Can we assume this will always be the case? Nope.
Can we assume these are the only types of URLs that will show up? Also nope.
That is why I rely on your help if you encounter URLs that don't match these patterns.
The worst that happens if the application gets this wrong, is that your folder is downloaded as a zip archive instead of being viewable in browser.
I wish the Dropbox client had this feature built in, but it just doesn't.
There may well be better ways to do this, I just made this to fix an annoyance of mine with the Dropbox client.
I won't provide a build of this.
You shouldn't download a prebuilt version of anything that modifies your clipboard contents without asking you.
I shouldn't need to explain why. And if you don't know why, don't download this repo.
