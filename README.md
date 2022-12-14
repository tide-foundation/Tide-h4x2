# The Tide H4X challenge
The [H4X challenge](http://h4x2.tide.org) is a showcase of the Tide Protocol's novel user authentication and digital protection technology, inviting the online community to learn, contribute and engage with Tide in the development of the protocol. It also encourages participants to identify and report security flaws, improvements or fixes via a bounty offer.

# H4X.2
This [H4X2](http://h4x2.tide.org) challenge is the second series of the community-engagement program by the [Tide Foundation](https://tide.org) with a specific focus on Tide's next-generation technology: A new technology to grant access using keys nobody holds. In this series, the challenge will change and evolve according to the community engagement, and will gradually introduce additional facets of the technology.

## Challenge Mechanics
The concept of the first challenge is simple.  A secret code is hidden and is only unlocked when the correct password is entered.  The first one to post the secret code on the Tide's #general channel on its discord server - wins!  The password authentication process is obfuscated and decentralized using Tide's [PRISM](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_prism.svg) cryptography - the world's most secure password authentication[^pwd].  In this challenge, only two nodes perform the authentication.  One node will be completely exposed and offers full transparancy to its internal data and processes while the other node remains private.  The entire source code for the challenge, together with full documentation, is offered herewith for those wishing to take a deeper look.  The user flow can be found below and the full technical diagram can be found [here](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_Challenge.svg).

## User Flow Diagram
![alt text](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_userflow.svg "Flow Diagram")

## Components
1. **H4x2-Node** - Small version of the Tide node specific to this challenge.
1. **H4x2-TinySDK** - Small SDK for frontend website integration.
1. **H4x2-front** - Frontend website for this challenge.
    1. **Modules/H4x2-TideJS** - Tide Libraries including encryption + PRISM
1. **diagrams** -  diagrams for this challenge.
    1. **H4x2_Challenge** - A technical diagram of the challenge.  
    2. **H4x2_prism** - The mathematical diagram of Tide's PRISM. 
    3. **H4x2_userflow** - User flow diagram. 

# Installation
This guide aims to assist you to replicate the entire challenge environment locally, with 2 nodes - so you can run it yourself freely.

While all the components of the this environment are cross-platform, this manual describes how to set it up in a Windows environment. Similar steps can be followed to achieve the same on Linux.

## Prerequisite

The following components are required to be set up ahead of the deployment:
1. [.NET 6 Build apps - SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0 ".net Core 6 Download")
1. Clone Repository (git clone https://github.com/tide-foundation/Tide-h4x2/)

## Deployment
### ORKs
````
cd folder_where_you_cloned_the_repo
cd Tide-h4x2\H4x2-Node\H4x2-Node
set ISPUBLIC=true
set PRISM_VAL=12345
dotnet run --urls=http://localhost:6001
````
Open another terminal
````
cd folder_where_you_cloned_the_repo
cd Tide-h4x2\H4x2-Node\H4x2-Node
set ISPUBLIC=false
set PRISM_VAL=67890
dotnet run --urls=http://localhost:7001
````
Much like the ORKs that are running in the cloud, both of these your ORKs have:
1. Different visibilities
2. Different PRISM values

To test this, navigate to http://localhost:6001/prizeKey. Notice how a value appears. In contrast, navigating to http://localhost:7001/prizeKey will show nothing, as the environment variable ISPUBLIC in the terminal set to false.

***NOTE: The reason we set one ORK to public is to show that even if one ORK is compromised, the user's key is still secure.***

### Static Web Page
````
cd Tide-h4x2\h4x2-front\js
````
In shifter.js, modify line 184 so that the front page will contact your local ORKs:
````
From this->  urls: ["https://h4x2-ork1.azurewebsites.net", "https://h4x2-ork2.azurewebsites.net"],

To this->    urls: ["http://localhost:6001", "http://localhost:7001"],
````
Now to host the webpage; this guide will use a simple python http server, but you can you anything you like.

Host the page with python:
````
python -m http.server 9000
````

Navigating to http://localhost:9000 will take you with the Tide H4x2 welcome page (the page with dots).

### Debug Web Page 
NOTE: This is only if you'd like to test your local ORKs with encryption/decryption of your own data with your own password

````
cd Tide-h4x2\h4x2-front\modules\H4x2-TideJS
````

If you look at the file \test\tests.js, you'll see a bunch of functions with different names, e.g. test1, test2...

These are tests we used for debugging purposes. They can also help you understand the different modules of Tide-JS such as AES, NodeClient and PrismFlow (when you just want to encrypt data).

Start a server in the directory where test.html is:
````
python -m http.server 8000
````

Navigate to http://localhost:8000/test.html where you'll see a VERY simple webpage.

Clicking each button will run the corresponding test in test.js. **The output of the function will be in the console.**

## Test
### Encrypting your own data
In the H4x2-TideJS directory:
1. In test/test.js, change "AAA" to any password of your choosing. Also change "Example" to anything you would like to encrypt.
4. Start a server (same location as test.html) so you can access http://localhost:8000/test.html
5. Visit the webpage and right-click -> inspect -> console
6. Click the button 'Test 4'
7. Should show a base64 encoded text in console

### Decrypting your own data
In the h4x2-front directory:
1. Modify the index.html file:
````
Change this line: <p hidden id="test">G4GmY31zIa35tEwck14URCEAIjeTA8NV+DgjHpngxASGnTU=</p>

To: <p hidden id="test">{Your base64 encrypted data from before}</p>
````
2. Start a server (same location as index.html) so you can access http://localhost:9000)
3. You should see the page with the dots.
4. Enter you password to see if it is able to decrypt!

Question: *So what was the data encrypted with?*

It was encrypted with the hash of a 'key point' only known to the *user who knows the password + has access to the ORKs*. 

In essence: ***key point = passwordPoint * (Prism1 + Prism2)***

Where passwordPoint is a point derived from the user's password. 

Even if someone knows Prism1, they still have to try every single possibilility for Prism2, which can get throttled by the ORK, hence lowering their probably of success by a LOT.

## Troubleshooting
Ask the discord for some help! The devs will be waiting.

# More info
[The Tide Website](https://tide.org)

## Get in touch!

[Tide Discord](https://discord.gg/42UCeW4smw)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)

  <a href="https://tide.org/licenses_tcosl-1-0-en">
    <img src="https://img.shields.io/badge/license-TCOS-green.svg" alt="license">
  </a>
</p>

[^pwd]: Tide has developed the world's most secure online password authentication mechanism because it is still, unfortunately, the most common online authentication mechanism to-date. In general, password authentication is a significantly inferior mechanism compared to its many alternatives. Most of the alternatives (e.g. MFA, passwordless, FIDO2, etc) also suffer from security risks which Tide's authentication helps alleviating. Tide's superior password protection mechanism isn't intended to discourage users from switching to a better alternatives, instead offers a better half-measure until such inevitable switch occurs.
