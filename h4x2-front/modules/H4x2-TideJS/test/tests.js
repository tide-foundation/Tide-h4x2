// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

import NodeClient from "../Clients/NodeClient.js";
import Point from "../Ed25519/point.js";
import PrismFlow from "../Flow/Prism.js";
import { decryptData, encryptData } from "../Tools/AES.js";
import { base64ToBytes, mod_inv } from "../Tools/Utils.js";

var tx = new TextEncoder();

export function test(){ // point function testing
  var b = 'uG7uZxtPjT+YBruVT3D/vml8kAs1yi713Mos/DcTbmN0GtOpZo3G1jQBRTIQz8JV2sMB3XT343U+LAsWk9b0Mw==';
  var f = base64ToBytes(b);
}

export async function test2(){ // initial client testing
  var nodeclient = new NodeClient('http://h4x2-ork2.azurewebsites.net', 'Test');
  var applied = await nodeclient.Apply(Point.fromB64('uG7uZxtPjT+YBruVT3D/vml8kAs1yi713Mos/DcTbmN0GtOpZo3G1jQBRTIQz8JV2sMB3XT343U+LAsWk9b0Mw=='));
  console.log(applied.toBase64());
  return applied.toBase64();
}

export async function test3(){
  var enc = await encryptData("Chickens", tx.encode("i2cxrwh+U+XGK6xOhqqbOHre2xTC6COAbwYBcuiOEW8D9tQSebQVSL932s7WTTPtXxPsvqx+fNJkvDBIPLSLFQ=="));
  console.log(enc);
}

export async function test4(){ // test AES encryptions
  
  var pass = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
  var data = "Chickens"
  var encrypted = await encryptData(data, tx.encode(pass));

  console.log(encrypted)

  var decrypted = await decryptData(encrypted, tx.encode(pass));
  console.log(decrypted);
}

export async function test6(){ // decryption test
  var config = {
    urls: ["http://h4x2-ork1.azurewebsites.net", "https://h4x2-ork2.azurewebsites.net"],
    encryptedData: ["zn7826Oa2t8VmPq0g0t5Amdsifve7uXm1JU/5p5TeQcE+jQ4N50dCkJpiJq++6qJA0Uo1w=="]
  }

  const flow = new PrismFlow(config);
  const decrypted = await flow.run("Julio's password");
  console.log(decrypted);
}

export async function test7(){ // full set up and decryption test
  var i;
  for(i = 0; i < 100; i++){
    var config = {
      urls: ["http://localhost:49156", "http://localhost:49156"],
      encryptedData: []
    }

    const flow = new PrismFlow(config);
    await flow.setUp("Julio's Password", "Prism", "Encrypt My Chickens");
    // At this point the message "Encrypt My Chickens" will be encrypted with the prismFlow using the password "Julio's Password"
    // The strength of this is that no-one can offline attack the AES encrypted data, even if the password is very short. The attacker MUST 
    // bruteforce by requesting the Test/Prize keys from the orks, which allows us to throttle and MAJORLY slow the attack.
    const decrypted = await flow.run("Julio's Password");
    console.log(decrypted);
  }
}

export function test8(){
  var r = BigInt("3140910109933614221592347532781209328214438396484525803322151119649026346873");
  var p = Point.g.times(r).times(BigInt("1554967558028019017635266871044954225154957206644506058050707959306264304941"));

  var p1 = p.times(r);

  var rinv = mod_inv(r);

  var p2 = p1.times(rinv);

  console.log(p.getX());
}
