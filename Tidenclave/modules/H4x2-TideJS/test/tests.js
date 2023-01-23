// 
// Tide Protocol - Infrastructure for a TRUE Zero-Trust paradigm
// Copyright (C) 2022 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Code License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Code License for more details.
// You should have received a copy of the Tide Community Open 
// Code License along with this program.
// If not, see https://tide.org/licenses_tcoc2-0-0-en
//


import NodeClient from "../Clients/NodeClient.js";
import Point from "../Ed25519/point.js";
import PrismFlow from "../Flow/Prism.js";
import { decryptData, encryptData } from "../Tools/AES.js";
import { BigIntToByteArray, RandomBigInt } from "../Tools/Utils.js";
import { SHA256_Digest } from "../Tools/Hash.js"
import { BigIntFromByteArray } from "../Tools/Utils.js"

var tx = new TextEncoder();

export async function test1(){ // initial client testing
  var nodeclient = new NodeClient('http://localhost:6001', 'Prism');
  const uid = BigIntFromByteArray(await SHA256_Digest("user")).toString();
  var applied = await nodeclient.Apply(uid, Point.fromB64('uG7uZxtPjT+YBruVT3D/vml8kAs1yi713Mos/DcTbmN0GtOpZo3G1jQBRTIQz8JV2sMB3XT343U+LAsWk9b0Mw=='));
  console.log(applied.toBase64());
}

export async function test2(){ // test Prism flow
  
  /**
   * @type {[string, Point][]}
   */
  var orkUrls = [['http://localhost', Point.fromB64('Ds5DKE6hxYNfpNcVRY4NCKznMxh9OwQ9bARan0w4qzbJo/hqrkZfDlZROGRRDzmXVh+iyeheoh3CKSMJ881gIg==')]];

  var pflow = new PrismFlow(orkUrls);
  var pass = "pass1";
  var pPoint = await Point.fromString(pass);

  var [state, sig] = await pflow.SetUp('myfirstuid5', pPoint, "encryptThis3");

  console.log(state)
  console.log(sig)
}

export async function test3(){ // full set up and decryption test
  /**
   * @type {[string, Point][]}
   */
  var orkUrls = [['http://localhost', Point.fromB64('Ds5DKE6hxYNfpNcVRY4NCKznMxh9OwQ9bARan0w4qzbJo/hqrkZfDlZROGRRDzmXVh+iyeheoh3CKSMJ881gIg==')]];

  var pflow = new PrismFlow(orkUrls);
  var pass = "pass1";
  var pPoint = await Point.fromString(pass);

  var CVK = await pflow.Authenticate('myfirstuid5', pPoint);

  var decrypted = await decryptData('9ZVVQCfJsQ5t6TJk7hNT+C97CQm6R3CovhAcVaPqMaoL+ViLxwux', BigIntToByteArray(CVK));

  console.log(decrypted)
}

export async function test4(){ // Test to encrypt data
  var config = {
    urls: ["http://localhost:6001", "http://localhost:7001"],
    encryptedData: []
  }

  const flow = new PrismFlow(config);

  await flow.setUp("Julio's UserName","AAA", "Prism", "Example")

  console.log(flow.encryptedData)
}

export async function test5(){ // test to decrypt data
  var config = {
    urls: ["http://localhost:6001", "http://localhost:7001"],
    encryptedData: ["G4GmY31zIa35tEwck14URCEAIjeTA8NV+DgjHpngxASGnTU="] // change this value
  }

  const flow = new PrismFlow(config)
  const decrypted = await flow.run("Julio's UserName","AAA")

  console.log(decrypted)
}

export async function getRandom(){
  alert(RandomBigInt().toString()) // don't let anyone tell you alert() is bad
}
