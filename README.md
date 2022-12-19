# The Tide H4X.2 challenge
The [H4X.2 challenge](http://h4x2.tide.org) is a showcase of the Tide Protocol's novel user authentication and digital protection technology, inviting the online community to learn, contribute and engage with Tide in the development of the protocol. It also encourages participants to identify and report security flaws, improvements or fixes via a bounty offer.

This challenge is the second series of the community-engagement program by the [Tide Foundation](https://tide.org) with a specific focus on Tide's next-generation technology: A new technology that grants access using keys **NOBODY** holds. Not even Tide! In this series, the challenge will change and evolve according to the community engagement, and will gradually introduce additional facets of the technology.

## Here's the 1st Challenge
The concept of the first challenge is simple.  A secret code is hidden and is only unlocked when the correct password is entered.  The first one to post the secret code on Tide's [#general](https://discord.com/channels/968760660659953714/1039488732639801414) channel on its Discord server - wins!  The password authentication process is obfuscated and decentralized using Tide's [PRISM](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_prism.svg) cryptography - the world's most secure password authentication[^pwd].  In this challenge, only two ORKs[^ork] perform the authentication.  One ORK will be completely exposed and offers full transparency to its internal data and processes while the other ORK remains private.  The entire source code for the challenge, together with full documentation, is offered herewith for those wishing to take a deeper look.  The user flow can be found below and the full technical diagram can be found [here](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_Challenge.svg).

## User Flow Diagram
![alt text](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_userflow.svg "Flow Diagram")

## Components
1. **H4x2-Node** - Minimal version of the Tide ORK, specific to this challenge.
1. **H4x2-TinySDK** - Minimal SDK for front-end website integration.
1. **H4x2-front** - Front-end website for this challenge.
    1. **Modules/H4x2-TideJS** - Tide Libraries including encryption + PRISM
1. **Diagrams** -  Diagrams for this challenge.
    1. [**H4x2_Challenge**](https://raw.githubusercontent.com/tide-foundation/Tide-h4x2/main/diagrams/svg/H4x2_Challenge.svg) - A technical diagram of the challenge.  
    2. [**H4x2_prism**](diagrams/svg/H4x2_prism.svg) - The mathematical diagram of Tide's PRISM. 
    3. [**H4x2_userflow**](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_userflow.svg) - A user flow diagram. 

# Installation
This guide aims to assist you in replicating the entire challenge environment locally, with 2 ORKs - so you can run it yourself freely.

While all the components of the environment are cross-platform, this manual describes how to set it up in a Windows environment. Similar steps can be followed to achieve the same on Linux.

There is also a [video](https://vimeo.com/780973408/d5df625214) to help you with the installation steps.

## Prerequisite

The following components are required to be set up ahead of the deployment:
1. [.NET 6 Build apps - SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0 ".net Core 6 Download")
1. Clone Repository (`git clone https://github.com/tide-foundation/Tide-h4x2/`)

## Deployment
### ORKs
Open a CMD terminal (not powershell)
````
cd Tide-h4x2\H4x2-Node\H4x2-Node
set ISPUBLIC=true
set PRISM_VAL=12345
dotnet run --urls=http://localhost:6001
````
Open another terminal
````
cd Tide-h4x2\H4x2-Node\H4x2-Node
set ISPUBLIC=false
set PRISM_VAL=67890
dotnet run --urls=http://localhost:7001
````
As you would want to generate cryptographically secure PRISM_VAL values, follow the [Debug Web Page](https://github.com/tide-foundation/Tide-h4x2#debug-web-page) steps to host the debug web page and click on the button 'Get Random'.

Much like the ORKs that are running in the cloud, both of your ORKs have:
1. Different visibilities
2. Different PRISM values

To test this, navigate to http://localhost:6001/prizeKey. Notice how a value appears. In contrast, navigating to http://localhost:7001/prizeKey will show PAGE NOT FOUND, as the environment variable ISPUBLIC in the terminal set to false.

***NOTE: The reason we set one ORK to public is to show that even if one ORK is compromised, the user's key is still entirely secure.***

### Static Web Page
Go to `Tide-h4x2\h4x2-front\js`

In `shifter.js`, modify line 184 so that the front-end page will contact your local ORKs:
From this: `urls: ["https://h4x2-ork1.azurewebsites.net", "https://h4x2-ork2.azurewebsites.net"],`
To this: `urls: ["http://localhost:6001", "http://localhost:7001"],`

Now to host the front-end webpage; this guide will use a simple Python http server, but you can you anything you like.

Host the page with Python:
````
python -m http.server 9000
````

Navigating to http://localhost:9000 will take you with the Tide H4x2 welcome page (similar to https://h4x2.tide.org).

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
In the H4x2-TideJS directory (Tide-h4x2\h4x2-front\modules\H4x2-TideJS):
1. In test4 function of test/test.js, change "AAA" to any password of your choosing. Also change "Example" to anything you would like to encrypt.
2. Go to http://localhost:8000/test.html and press F5 (to reload the page)
3. Right-click -> inspect -> console
4. Click the button 'Test 4'
5. Should show a base64 encoded text in console

### Decrypting your own data
In the h4x2-front directory:
1. Modify the index.html file:

    Change this line: `<p hidden id="test">G4GmY31zIa35tEwck14URCEAIjeTA8NV+DgjHpngxASGnTU=</p>`
    
    To: `<p hidden id="test">{Your base64 encrypted data from before}</p>`

2. Go to http://localhost:9000) and press F5 (to reload the page)
3. You should see the page with the dots.
4. Enter your password to see if it is able to decrypt!

Question: *So what was the data encrypted with?*

It was encrypted with the hash of a 'key point'[^key] only known to the *user who knows the password + has access to the ORKs*.

In essence: ***key point = passwordPoint * (Prism1 + Prism2)***

Where passwordPoint is a point derived from the user's password. 

Even if someone knows Prism1, they still have to try virtually infinite possibilities for Prism2, which will be throttled by the ORK, hence lowering their probably of success to virtually zero.

## Troubleshooting
Ask for any help in the Discord channel! The community and our devs are there for you.

## A Quick Note on the Throttling
You may notice that regardless if you entered the right password or not, the ORKs will throttle you after few attempts. This is due to the fact that it is virtually IMPOSSIBLE (unless you break Eliptic Curve cryptography) for the ORKs to determine what password the user is trying and whether its correct or not (specifically, in this challenge). All the ORKs do is apply their partial PRISM value to a point. Therefore, since the ORKs have no idea what the password is and since the user is obfuscating their password point with a random number, it guarantees that the ORKs 'authenticate' the user without any knowledge of their password. Cool, right?

# More info
[The Tide Website](https://tide.org)

## Get in touch!

[Tide Discord](https://discord.gg/42UCeW4smw)

[Tide Twitter](https://twitter.com/tidefoundation)

  <a href="https://tide.org/licenses_tcoc2-0-0-en">
    <img src="https://img.shields.io/badge/license-TCOC-green.svg" alt="license">
  </a>
</p>

[^pwd]: Tide's focus on developing the world's most secure online password authentication mechanism is because passwords still, unfortunately, are the most common online authentication mechanism used. In general, password authentication is a significantly inferior mechanism compared to its many alternatives. Most of the alternatives (e.g. MFA, passwordless, FIDO2, etc) also suffer from security risks which Tide's authentication helps alleviate. Tide's superior password protection mechanism isn't intended to discourage users from switching to a better alternative, instead offers a better interim-measure until such inevitable switch occurs.
[^ork]: Tide's decentralized network is made of many nodes named ORKs, which stands for Orchestrated Recluder of Keys. A single ORK operates more like a drone in a hive than a node in a network as it performs work that's unique to it and is vastly different than other ORKs. That work is entirely incomprehensive by itself, even to itself. Meaning, the network perform a process where each ORK performs part of that process without knowing or understanding anything about the process itself. Only after the ORKs complete their parts (which is done in parallel), the network produces a meaningful result. This "incomprehensible partial processing", or as we call it "Blind Secret Processing" is done using Tide's groundbreaking new Threshold Cryptography done in Multi-Party Computation.
[^key]: Tide's specific 'key point' is a representation of a cryptographic key as a geometric point on an Edward25519 Elliptic Curve.
