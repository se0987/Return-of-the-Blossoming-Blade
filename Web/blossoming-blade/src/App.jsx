import './App.css';
import blossoming from "./assets/blossoming.png"
import maehwa from "./assets/maehwa.png"
import React from 'react';
import styled from 'styled-components'
import { Link } from 'react-router-dom';

function App() {
  const url = "http://k9d107.p.ssafy.io/downloads/ReturnOfTheBlossomingBlade.zip"

  return (
    <>
    <ImgContainer>
      <img alt="main_image" src={blossoming}/>
      <div>화산귀환</div>
      <p>알려지지 않은 이야기</p>
      <a href={url}>다운로드</a>
    </ImgContainer>
      <MaehwaContainer>
      <img src={maehwa} alt="maehwa" />
    </MaehwaContainer>
    </>
  );
}

export default App;

const ImgContainer = styled.div`

@keyframes spreadText {
  from {
    letter-spacing: normal;
  }
  to {
    letter-spacing: 10px;
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

  width: 100%;
  & > img {
    width: 100%;
    position: relative;
    pointer-events: none;
    height: 99.58vh;
  }
  & > div{
    font-size: 70px;
    position: absolute;
    top: 25%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: #f6deee;
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
    pointer-events: none;
    animation: spreadText 2s ease-out forwards;
    z-index: 4;
  }

  & > p{
    font-size: 60px;
    position: absolute;
    top: 30%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: #ffffff;
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
    pointer-events: none;
    animation: fadeIn 2s ease-in forwards;
    animation-delay: 1s;  
    opacity: 0;
    z-index: 2;
  }
  & > button{
    font-size: 50px;
    position: absolute;
    top: 103%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: black;
    background-color: none;
  }

  & > a{
    font-size: 60px;
    position: absolute;
    top: 76%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: black;
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
    cursor: pointer;
    user-select: none;
    text-decoration: none;

    &:hover {
    color: #E86B79;
  }
  }
  @font-face {
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
}
`

const MaehwaContainer = styled.div`

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
  position: absolute;
  margin-top: -12%;
  top: 25%;
  left: 34%;
  width: 30%;
  height: auto;

  animation: fadeIn 2s ease-in forwards;
  z-index: 1;
  /* opacity: 0; */

  img {
    position: absolute;
    width: 100%;
    height: auto;
    z-index:1;
  }
`;